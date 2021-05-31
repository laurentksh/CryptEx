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

        Task RequestPasswordChange(AppUser user, RequestPasswordChangeDTO passwordChangeDTO);

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

        public async Task RequestPasswordChange(AppUser user, RequestPasswordChangeDTO passwordChangeDTO)
        {
            if (passwordChangeDTO is null)
                throw new ArgumentNullException(nameof(passwordChangeDTO));

            var token = await userManager.GeneratePasswordResetTokenAsync(user); //Send this by email

            //TODO: Implement, also use a mail sender like MailChimp. Or just display the password change token on screen.
        }

        public async Task ChangePassword(AppUser user, ChangePasswordDTO changePasswordDTO)
        {
            if (changePasswordDTO is null)
                throw new ArgumentNullException(nameof(changePasswordDTO));

            var result = await userManager.ResetPasswordAsync(user, changePasswordDTO.Token, changePasswordDTO.NewPassword);

            if (!result.Succeeded) {
                throw new IdentityException(result.Errors.ToList());
            }
        }
    }
}
