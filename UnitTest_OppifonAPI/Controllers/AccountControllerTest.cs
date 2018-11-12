using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Factory;
using DAL.Models;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OppifonAPI.Controllers;
using OppifonAPI.DTO;
using OppifonAPI.Helpers;

namespace UnitTest_OppifonAPI.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private AccountController _uut;
        private Mock<IFactory> _factoryMock;
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<IPasswordHasher<User>> _passwordHasherMock;
        private Mock<IUserValidator<User>> _userValidatorMock;
        private Mock<IPasswordValidator<User>> _passwordValidatorMock;
        private Mock<ILookupNormalizer> _lookupNormalizerMock;
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IUserClaimsPrincipalFactory<User>> _userClaimsPrincipalFactoryMock;
        private Mock<IUnityOfWork> _unitOfWorkMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<SignInManager<User>> _signInManagerMock;
        private Mock<IUserRepository> _userRepository;
        private Mock<IExpertRepository> _expertRepositoryMock;
        private Mock<ICategoryRepository> _categoryMock;
        private Mock<ITagRepository> _tagRepositoryMock;
        // SignInManager<TUser>(UserManager<TUser>, IHttpContextAccessor, IUserClaimsPrincipalFactory<TUser>, IOptions<IdentityOptions>, ILogger<SignInManager<TUser>>, IAuthenticationSchemeProvider)
        // UserManager<TUser>(IUserStore<TUser>, IOptions<IdentityOptions>, IPasswordHasher<TUser>, IEnumerable<IUserValidator<TUser>>, IEnumerable<IPasswordValidator<TUser>>, ILookupNormalizer, IdentityErrorDescriber, IServiceProvider, ILogger<UserManager<TUser>>)

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnityOfWork>();
            _factoryMock = new Mock<IFactory>();
            _factoryMock.Setup(x => x.GetUOF()).Returns(_unitOfWorkMock.Object);

            _userStoreMock = new Mock<IUserStore<User>>();
            _passwordValidatorMock = new Mock<IPasswordValidator<User>>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _userValidatorMock = new Mock<IUserValidator<User>>();
            _lookupNormalizerMock = new Mock<ILookupNormalizer>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userStoreMock = new Mock<IUserStore<User>>();
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
            _userRepository = new Mock<IUserRepository>();
            _expertRepositoryMock = new Mock<IExpertRepository>();
            _categoryMock = new Mock<ICategoryRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();

            _userManagerMock = new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object, _httpContextAccessorMock.Object, _userClaimsPrincipalFactoryMock.Object, null, null, null);

            
        }

        [TestMethod]
        public async Task Register_User_Ok()
        {
            // Arrange
            var dtoUser = new DTORegisterUser
            {
                FirstName = "John",
                Password = "JohnJohn1234",
                LastName = "Doe",
                Birthday = DateTime.Now,
                Email = "JohnDoe@emai.com",
                City = "Dublin",
                Gender = "Male",
                IsExpert = false,
                PhoneNumber = "1234567890"
            };

            var user = new User();
            Mapper.Map(dtoUser, user);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            _userRepository.Setup(x => x.GetByEmail(It.IsAny<string>())).Returns(user);
            _unitOfWorkMock.Setup(x => x.Users).Returns(_userRepository.Object);
            _uut = new AccountController(_factoryMock.Object, _userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = await _uut.Register(dtoUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task Register_Expert_Ok()
        {
            // Arrange
            var dtoUser = new DTORegisterUser
            {
                FirstName = "John",
                Password = "JohnJohn1234",
                LastName = "Doe",
                Birthday = DateTime.Now,
                Email = "JohnDoe@emai.com",
                City = "Dublin",
                Gender = "Male",
                IsExpert = true,
                PhoneNumber = "1234567890",
                ExpertCategory = "Computer Science",
                ExpertTags = new List<string> { "Computers" },
                MainFields = new List<string> { "Computers" }
            };

            var category = new Category
            {
                Name = dtoUser.ExpertCategory,
                Experts = new List<Expert>(),
                Tags =  new List<Tag>()
            };

            var user = new User();
            var expert = new Expert();
            Mapper.Map(dtoUser, user);
            Mapper.Map(user, expert);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

            _userRepository.Setup(x => x.GetByEmail(It.IsAny<string>())).Returns(user);
            _unitOfWorkMock.Setup(x => x.Users).Returns(_userRepository.Object);

            _expertRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).Returns(expert);
            _unitOfWorkMock.Setup(x => x.Experts).Returns(_expertRepositoryMock.Object);

            _categoryMock.Setup(x => x.GetCategoryEagerByName(It.IsAny<string>())).Returns(category);
            _unitOfWorkMock.Setup(x => x.Categories).Returns(_categoryMock.Object);

            _tagRepositoryMock.Setup(x => x.GetTagByName(It.IsAny<string>()));
            _unitOfWorkMock.Setup(x => x.Tags).Returns(_tagRepositoryMock.Object);

            _uut = new AccountController(_factoryMock.Object, _userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = await _uut.Register(dtoUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
