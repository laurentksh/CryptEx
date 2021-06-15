using CryptExApi.Data;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using CryptExApi.Repositories;
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
    public class UserServiceTests
    {
        private IUserService userService;
        private AppUser fakeUser;
        private readonly Guid fakeUserId = Guid.NewGuid();
        private const string fakeUserPassword = "Password123$";

        [TestInitialize]
        public void Setup()
        {
            fakeUser = new AppUser()
            {
                Id = fakeUserId,
                UserName = Guid.NewGuid().ToString(),
                FirstName = "Test",
                LastName = "Account",
                Email = "testaccount@cryptex-trade.tech",
                BirthDay = DateTime.Today.AddYears(-20)
            };
            
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
            var userRepo = new Mock<IUserRepository>();
            

            //UserRepository Mock
            userRepo.Setup(x => x.GetUser(It.Is<Guid>(x => x == fakeUserId)))
                .ReturnsAsync(fakeUser);

            userRepo.Setup(x => x.ChangeLanguage(It.IsAny<AppUser>(), It.IsAny<string>()));

            userRepo.Setup(x => x.ChangeCurrency(It.IsAny<AppUser>(), It.IsAny<string>()));

            userRepo.Setup(x => x.GetAddress(It.IsAny<AppUser>()))
                .ReturnsAsync(AddressViewModel.FromAddress(new UserAddress
                {
                    Id = Guid.NewGuid(),
                    Street = "Abc 1",
                    PostalCode = "1234",
                    City = "CityName",
                    CreationDate = DateTime.Now.AddDays(-5),
                    CountryId = Guid.NewGuid(),
                    Country = new Country { Id = Guid.NewGuid(), Iso31661Alpha2Code = "CH" },
                    User = fakeUser,
                    UserId = fakeUser.Id
                }));

            userRepo.Setup(x => x.GetIban(It.IsAny<AppUser>()))
                .ReturnsAsync(new BankAccountViewModel
                {
                    Iban = "ApprovedIban",
                    Status = BankAccountStatus.Approved
                });

            //UserManager Mock
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

            userManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<AppUser>()))
                .ReturnsAsync("ResetToken");

            userManager.Setup(x => x.ResetPasswordAsync(It.IsAny<AppUser>(), It.Is<string>(x => x == "ResetToken"), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userService = new UserService(userManager.Object, userRepo.Object);
        }

        [TestMethod]
        public void GetUser()
        {
            var user = userService.GetUser(fakeUser.Id).GetAwaiter().GetResult();

            Assert.IsNotNull(user);
            Assert.AreEqual(fakeUser.Id, user.Id);
        }

        [TestMethod]
        public void UpdateUser()
        {
            var result = userService.UpdateUser(fakeUser, new CryptExApi.Models.DTO.UpdateUserDto()).GetAwaiter().GetResult();

            Assert.IsNotNull(result);
            Assert.AreEqual(fakeUser.Id, result.Id);
        }

        [TestMethod]
        public void ChangeLanguage()
        {
            userService.ChangeLanguage(fakeUser, "fr-fr").GetAwaiter().GetResult();
        }

        [TestMethod]
        public void ChangeCurrency()
        {
            userService.ChangeCurrency(fakeUser, "CHF").GetAwaiter().GetResult();
        }

        [TestMethod]
        public void RequestPasswordChange()
        {
            var token = userService.RequestPasswordChange(fakeUser, new CryptExApi.Models.DTO.RequestPasswordChangeDTO() { Email = fakeUser.Email })
                .GetAwaiter()
                .GetResult();

            Assert.AreEqual("ResetToken", token);
        }

        [TestMethod]
        public void ChangePassword()
        {
            userService.ChangePassword(fakeUser, new CryptExApi.Models.DTO.ChangePasswordDTO
            {
                Email = fakeUser.Email,
                NewPassword = "NewPassword123$",
                Token = "ResetToken"
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void GetAddress()
        {
            var address = userService.GetAddress(fakeUser).GetAwaiter().GetResult();

            Assert.IsNotNull(address);

            Assert.AreEqual("CityName", address.City);
            Assert.AreEqual("CH", address.Country.Iso31661Alpha2Code);
        }

        [TestMethod]
        public void GetIban()
        {
            var iban = userService.GetIban(fakeUser).GetAwaiter().GetResult();

            Assert.IsNotNull(iban);
            Assert.AreEqual("ApprovedIban", iban.Iban);
        }

        [TestMethod]
        public void SetAddress()
        {
            userService.SetAddress(fakeUser, new CryptExApi.Models.DTO.AddressDto()).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void SetIban()
        {
            userService.SetIban(fakeUser, new CryptExApi.Models.DTO.IbanDto()).GetAwaiter().GetResult();
        }
    }
}
