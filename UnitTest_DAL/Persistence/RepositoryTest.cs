using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Data;
using DAL.Models;
using DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest_DAL.Persistence
{
    [TestClass]
    public class RepositoryTest
    {
        private DbContextOptions<Context> _options;
        private Context _context;
        private Repository<User> _uut;


        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "Oppifon")
                .Options;

            _context = new Context(_options);
            _uut = new Repository<User>(_context);
        }

        [TestMethod]
        public void Get_EntityNotInDatabase_ReturnsNull()
        {
            // Arrange
            // Act
            var result = _uut.Get(Guid.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Get_EntityInDatabase_ReturnsEntity()
        {
            // Arrange
            var user = new User();
            _uut.Add(user);

            // Act
            var result = _uut.Get(user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void GetAll_EntityNotInDatabase_EmptyList()
        {
            // Arrange
            
            // Act
            var result = _uut.GetAll().ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAll_EntitiesInDatabase_ListContainsTwoElements()
        {
            // Arrange
            _uut.Add(new User());
            _uut.Add(new User());
            _context.SaveChanges();

            // Act
            var result = _uut.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Find_EntitiesInDatabase_ListContainsTwoElements()
        {
            // Arrange
            const string firstName = "Hans";
            _uut.Add(new User{FirstName = firstName});
            _uut.Add(new User{FirstName = firstName});

            // Act
            var result = _uut.Find(x => x.FirstName == firstName);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void SingleOrDefault_EntityInDatabase_ReturnsEntity()
        {
            // Arrange
            var user = new User{FirstName = "Hans"};
            _uut.Add(user);

            // Act
            var result = _uut.SingleOrDefault(x => x.FirstName == user.FirstName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void AddRange_EntityInDatabase_ReturnsEntity()
        {
            // Arrange
            var users = new List<User> {new User(), new User()};

            // Act
            _uut.AddRange(users);

            // Assert
            Assert.AreEqual(2, _uut.GetAll().Count());
        }

        [TestMethod]
        public void Update_EntityInDatabase_ReturnsEntity()
        {
            // Arrange
            var user = new User();
            _uut.Add(user);
            
            // Act
            user.FirstName = "Hans";
            _uut.Update(user);

            // Assert
            var result = _uut.GetAll();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(user, result.First());
        }

        [TestMethod]
        public void Remove_EntityInDatabase_ReturnsEntity()
        {
            // Arrange
            var user = new User();
            _uut.Add(user);

            // Act
            _uut.Remove(user);

            // Assert
            var result = _uut.GetAll();

            Assert.AreEqual(0, result.Count());
            
        }

        [TestMethod]
        public void RemoveRange_EntityInDatabase_ReturnsEntity()
        {
            // Arrange
            var users = new List<User> {new User(), new User()};
            _uut.AddRange(users);

            // Act
            _uut.RemoveRange(users);

            // Assert
            Assert.AreEqual(0, _uut.GetAll().Count());
        }
    }
}
