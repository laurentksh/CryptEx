using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Exceptions;
using CryptExApi.Models.Database;
using CryptExApi.Models.DTO;
using CryptExApi.Models.ViewModel;
using CryptExApi.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CryptExApi.Services
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser(Guid id);

        Task ChangeLanguage(AppUser user, string language);

        Task ChangeCurrency(AppUser user, string currency);

        Task<string> RequestPasswordChange(AppUser user, RequestPasswordChangeDTO passwordChangeDTO);

        Task ChangePassword(AppUser user, ChangePasswordDTO changePasswordDTO);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager; //User Manager acts as a repository.
        private readonly IUserRepository userRepository;

        public UserService(UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        public async Task<UserViewModel> GetUser(Guid id)
        {
            return UserViewModel.FromAppUser(await userRepository.GetUser(id));
        }

        public async Task ChangeLanguage(AppUser user, string language)
        {
            await userRepository.ChangeLanguage(user, language);
        }

        public async Task ChangeCurrency(AppUser user, string currency)
        {
            await userRepository.ChangeCurrency(user, currency);
        }

        public async Task<string> RequestPasswordChange(AppUser user, RequestPasswordChangeDTO passwordChangeDTO)
        {
            if (passwordChangeDTO is null)
                throw new ArgumentNullException(nameof(passwordChangeDTO));

            
            user ??= await userManager.FindByEmailAsync(passwordChangeDTO.Email);
            if (user == null)
                throw new NotFoundException("Email not found.");

            var token = await userManager.GeneratePasswordResetTokenAsync(user); //Send this by email

            //We should be using a mail provider to send the token to the user but since this is a school project we won't do that.
            return token;
        }

        public async Task ChangePassword(AppUser user, ChangePasswordDTO changePasswordDTO)
        {
            if (changePasswordDTO is null)
                throw new ArgumentNullException(nameof(changePasswordDTO));

            user ??= await userManager.FindByEmailAsync(changePasswordDTO.Email);
            if (user == null)
                throw new NotFoundException("Email not found.");

            var result = await userManager.ResetPasswordAsync(user, changePasswordDTO.Token, changePasswordDTO.NewPassword);

            if (!result.Succeeded) {
                throw new IdentityException(result.Errors.ToList());
            }
        }
    }
}
