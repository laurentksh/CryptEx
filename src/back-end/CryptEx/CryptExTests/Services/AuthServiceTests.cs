using CryptExApi.Models.Database;
using CryptExApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CryptExTests.Services
{
    [TestClass]
    public class AuthServiceTests
    {
        private const string jwtKey = "abcdef1234567890$abcdef1234567890$abcdef1234567890$abcdef1234567890$";
        private IAuthService authService;
        private AppUser fakeUser;
        private const string fakeUserPassword = "Password123$";

        [TestInitialize]
        public void Setup()
        {
            var settings = new Dictionary<string, string>
            {
                { "JwtSigningKey", jwtKey }
            };

            fakeUser = new AppUser()
            {
                Id = Guid.NewGuid(),
                UserName = Guid.NewGuid().ToString(),
                FirstName = "Test",
                LastName = "Account",
                Email = "testaccount@cryptex-trade.tech"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
            
            //var mockedUM = new UserManager<AppUser>(
            //        new Mock<IUserStore<AppUser>>().Object,
            //        new Mock<IOptions<IdentityOptions>>().Object,
            //        new Mock<IPasswordHasher<AppUser>>().Object,
            //        new Mock<IEnumerable<IUserValidator<AppUser>>>().Object,
            //        new Mock<IEnumerable<IPasswordValidator<AppUser>>>().Object,
            //        new Mock<ILookupNormalizer>().Object,
            //        new Mock<IdentityErrorDescriber>().Object,
            //        new Mock<IServiceProvider>().Object,
            //        new Mock<ILogger<UserManager<AppUser>>>().Object
            //        );

            var userManager = new Mock<UserManager<AppUser>>(new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByEmailAsync(It.Is(fakeUser.Email, StringComparer.InvariantCulture)))
                .ReturnsAsync(fakeUser);

            userManager.Setup(x => x.CheckPasswordAsync(It.Is<AppUser>(x => fakeUser == x), It.Is<string>(x => fakeUserPassword == x)))
                .ReturnsAsync(true);

            userManager.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(new List<string> { "user" });

            userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            authService = new AuthService(userManager.Object, config);
        }

        [TestMethod]
        public void Authenticate()
        {
            var vm = authService.Authenticate(new CryptExApi.Models.DTO.AuthDTO
            {
                Email = fakeUser.Email,
                Password = fakeUserPassword,
                ExtendSession = false
            }).GetAwaiter().GetResult();

            Assert.IsNotNull(vm);

            ValidateJwt(vm.JWToken, fakeUser.Id.ToString(), fakeUser.Email);
        }

        [TestMethod]
        public void CreateUser()
        {
            var user = authService.CreateUser(new CryptExApi.Models.DTO.CreateUserDTO
            {
                FirstName = "Laurent",
                LastName = "Keusch",
                BirthDay = new DateTime(2002, 03, 01),
                Email = "email@example.com",
                Password = "Password123$"
            }).GetAwaiter().GetResult();

            ValidateJwt(user.JWToken, null, "email@example.com");
        }

        [TestMethod]
        public void BuildJWT()
        {
            var userId = Guid.NewGuid();
            var email = "abc@def.com";

            var user = new AppUser()
            {
                Id = userId,
                UserName = userId.ToString(),
                Email = email,
                FirstName = "abc",
                LastName = "def"
            };

            var roles = new List<string>
            {
                "user",
                "admin"
            };

            var jwt = authService.BuildJWT(user, false, null, roles).GetAwaiter().GetResult();

            var token = ValidateJwt(jwt, userId.ToString(), email);
            Assert.IsTrue(token.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "admin"));
        }

        private static JwtSecurityToken ValidateJwt(string jwt, string userId, string email)
        {
            var handler = new JwtSecurityTokenHandler();

            var valid = handler.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateAudience = false,
                RequireExpirationTime = true,

                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512Signature },
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey))
            }, out _);

            Assert.IsNotNull(valid);

            var token = handler.ReadJwtToken(jwt);

            if (userId != null)
                Assert.IsTrue(token.Claims.Any(x => x.Type == ClaimTypes.NameIdentifier && x.Value == userId.ToString()));
            
            if (email != null)
                Assert.IsTrue(token.Claims.Any(x => x.Type == JwtRegisteredClaimNames.Email && x.Value == email));
            
            Assert.IsTrue(token.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "user"));

            return token;
        }
    }
}
