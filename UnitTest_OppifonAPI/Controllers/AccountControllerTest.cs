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
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace UnitTest_OppifonAPI.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        // uut
        private AccountController _uut;

        // Controller injection
        private Mock<IFactory> _factoryMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<SignInManager<User>> _signInManagerMock;

        // Manager dependencies
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IUserClaimsPrincipalFactory<User>> _userClaimsPrincipalFactoryMock;

        // Repositories
        private Mock<IUnityOfWork> _unitOfWorkMock;
        private Mock<IUserRepository> _userRepository;
        private Mock<IExpertRepository> _expertRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<ITagRepository> _tagRepositoryMock;
       
        [TestInitialize]
        public void Setup()
        {
            // Repositories
            _unitOfWorkMock = new Mock<IUnityOfWork>();
            _userRepository = new Mock<IUserRepository>();
            _expertRepositoryMock = new Mock<IExpertRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();

            // Manager dependencies
            _userStoreMock = new Mock<IUserStore<User>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
           
            // Controller injections
            _userManagerMock = new Mock<UserManager<User>>(_userStoreMock.Object, 
                null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object,
                _httpContextAccessorMock.Object, _userClaimsPrincipalFactoryMock.Object, null, null, null);

            _factoryMock = new Mock<IFactory>();
            _factoryMock.Setup(x => x.GetUOF()).Returns(_unitOfWorkMock.Object);
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
                PhoneNumber = "1234567890",
                InterestTags = new List<string> { "Phone" }
            };

            var user = new User();
            Mapper.Map(dtoUser, user);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

            _userRepository.Setup(x => x.GetByEmail(It.IsAny<string>())).Returns(user);
            _unitOfWorkMock.Setup(x => x.Users).Returns(_userRepository.Object);

            _tagRepositoryMock.Setup(x => x.GetTagByName(It.IsAny<string>()));
            _unitOfWorkMock.Setup(x => x.Tags).Returns(_tagRepositoryMock.Object);

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

            _categoryRepositoryMock.Setup(x => x.GetCategoryEagerByName(It.IsAny<string>())).Returns(category);
            _unitOfWorkMock.Setup(x => x.Categories).Returns(_categoryRepositoryMock.Object);

            _tagRepositoryMock.Setup(x => x.GetTagByName(It.IsAny<string>()));
            _unitOfWorkMock.Setup(x => x.Tags).Returns(_tagRepositoryMock.Object);

            _uut = new AccountController(_factoryMock.Object, _userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = await _uut.Register(dtoUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task Register_NoExpertTags_BadRequest()
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
                MainFields = new List<string> { "Computers" }
            };

            var category = new Category
            {
                Name = dtoUser.ExpertCategory,
                Experts = new List<Expert>(),
                Tags = new List<Tag>()
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

            _categoryRepositoryMock.Setup(x => x.GetCategoryEagerByName(It.IsAny<string>())).Returns(category);
            _unitOfWorkMock.Setup(x => x.Categories).Returns(_categoryRepositoryMock.Object);

            _tagRepositoryMock.Setup(x => x.GetTagByName(It.IsAny<string>()));
            _unitOfWorkMock.Setup(x => x.Tags).Returns(_tagRepositoryMock.Object);

            _uut = new AccountController(_factoryMock.Object, _userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = await _uut.Register(dtoUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Register_NoMainFields_BadRequest()
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
                ExpertTags = new List<string> { "Computers" }
            };

            var category = new Category
            {
                Name = dtoUser.ExpertCategory,
                Experts = new List<Expert>(),
                Tags = new List<Tag>()
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

            _categoryRepositoryMock.Setup(x => x.GetCategoryEagerByName(It.IsAny<string>())).Returns(category);
            _unitOfWorkMock.Setup(x => x.Categories).Returns(_categoryRepositoryMock.Object);

            _tagRepositoryMock.Setup(x => x.GetTagByName(It.IsAny<string>()));
            _unitOfWorkMock.Setup(x => x.Tags).Returns(_tagRepositoryMock.Object);

            _uut = new AccountController(_factoryMock.Object, _userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = await _uut.Register(dtoUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Login_ValidUser_Ok()
        {
            // Arrange
            var user = new User
            {
                Email = "",
                FirstName = "",
                LastName = "",
                Id = Guid.NewGuid(),
                City = "",
                Birthday = DateTime.Now,
                Gender = "",
                IsExpert = false
            };

            var dtoLoginUser = new DTOLoginUser
            {
                Email = It.IsAny<string>(),
                Password = It.IsAny<string>()
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false))
                .Returns(Task.FromResult(SignInResult.Success));

            _uut = new AccountController(_factoryMock.Object, _userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = await _uut.Login(dtoLoginUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Login_InValidEmail_BadRequest()
        {
            // Arrange
            var user = new User
            {
                Email = "",
                FirstName = "",
                LastName = "",
                Id = Guid.NewGuid(),
                City = "",
                Birthday = DateTime.Now,
                Gender = "",
                IsExpert = false
            };

            var dtoLoginUser = new DTOLoginUser
            {
                Email = It.IsAny<string>(),
                Password = It.IsAny<string>()
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult((User)null));
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false))
                .Returns(Task.FromResult(SignInResult.Failed));

            _uut = new AccountController(_factoryMock.Object, _userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = await _uut.Login(dtoLoginUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Login_InValidPassword_BadRequest()
        {
            // Arrange
            var user = new User
            {
                Email = "",
                FirstName = "",
                LastName = "",
                Id = Guid.NewGuid(),
                City = "",
                Birthday = DateTime.Now,
                Gender = "",
                IsExpert = false
            };

            var dtoLoginUser = new DTOLoginUser
            {
                Email = It.IsAny<string>(),
                Password = It.IsAny<string>()
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false))
                .Returns(Task.FromResult(SignInResult.Failed));

            _uut = new AccountController(_factoryMock.Object, _userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = await _uut.Login(dtoLoginUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
