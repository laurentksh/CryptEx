using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;
using CryptExApi.Models.DTO;
using CryptExApi.Models.ViewModel;
using CryptExApi.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CryptExApi.Services
{
    public interface IAuthService
    {
        Task<AuthViewModel> Authenticate(AuthDTO authDTO);

        Task<AuthViewModel> CreateUser(CreateUserDTO createUserDTO);

        Task<string> BuildJWT(AppUser user, bool prolongedSession = false, IEnumerable<Claim> claims = null, IEnumerable<string> roles = null);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;

        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<AuthViewModel> Authenticate(AuthDTO authDTO)
        {
            if (authDTO == null)
                throw new ArgumentNullException(nameof(authDTO));

            var user = await userManager.FindByEmailAsync(authDTO.Email);

            if (user == null)
                throw new NotFoundException();

            var loggedIn = await userManager.CheckPasswordAsync(user, authDTO.Password);
            
            if (!loggedIn)
                throw new UnauthorizedException();

            var claims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            var token = await BuildJWT(user, authDTO.ExtendSession, claims, roles);

            return new AuthViewModel(token);
        }

        public async Task<AuthViewModel> CreateUser(CreateUserDTO createUserDTO)
        {
            if (createUserDTO == null)
                throw new ArgumentNullException(nameof(createUserDTO));

            var user = new AppUser
            {
                Email = createUserDTO.Email,
                UserName = StringUtilities.SecureRandom(16, StringUtilities.AllowedChars.AlphabetNumbers), // We do not use usernames, so we generate a random one so that Identity doesn't throw errors.
                FirstName = createUserDTO.FirstName,
                LastName = createUserDTO.LastName,
                BirthDay = createUserDTO.BirthDay
            };

            var result = await userManager.CreateAsync(user, createUserDTO.Password);
            
            if (!result.Succeeded) {
                throw new IdentityException(result.Errors.ToList());
            }

            await userManager.AddToRoleAsync(user, "user");

            var roles = new List<string>() { "user" };
            
            var token = await BuildJWT(user, false, roles: roles);

            return new AuthViewModel(token);
        }

        /*public async Task<string> BuildJWT(AppUser user, bool prolongedSession = false, IEnumerable<Claim> claims = null, IEnumerable<string> roles = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtSigningKey"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //new Claim(JwtRegisteredClaimNames.Sub, user.Email), //.NET will map this to NameIdentifier for some reason...
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }),
                Claims = new Dictionary<string, object>(),
                Expires = prolongedSession ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            if (claims is not null)
                foreach (var claim in claims)
                    tokenDescriptor.Claims.Add(claim.Type, claim.Value);

            if (roles is not null)
                foreach (var role in roles)
                    tokenDescriptor.Claims.Add(ClaimTypes.Role, role);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }*/

        public async Task<string> BuildJWT(AppUser user, bool prolongedSession = false, IEnumerable<Claim> inClaims = null, IEnumerable<string> roles = null) //New method that supports having multiple roles
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtSigningKey"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Sub, user.Email)), //.NET will map this to NameIdentifier for some reason...
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            if (inClaims is not null)
                claims.AddRange(inClaims);

            if (roles is not null) {
                foreach (var role in roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expiration = prolongedSession ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(4);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            );

            return tokenHandler.WriteToken(token);
        }
    }
}
