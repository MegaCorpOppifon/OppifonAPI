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

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
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
            SaveChanges();

            // Act
            var result = _uut.Get(user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void GetAll_NoEntitiesInDatabase_ReturnsEmptyList()
        {
            // Arrange
            
            // Act
            var result = _uut.GetAll();

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetAll_EntitiesInDatabase_ListContainsTwoElements()
        {
            // Arrange
            _uut.Add(new User());
            _uut.Add(new User());
            SaveChanges();

            // Act
            var result = _uut.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Find_NoEntitiesInDatabase_ReturnsEmptyList()
        {
            // Arrange
            
            // Act
            var result = _uut.Find(x => x.FirstName == "hans");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Find_EntitiesInDatabase_ListContainsTwoElements()
        {
            // Arrange
            const string firstName = "Hans";
            _uut.Add(new User { FirstName = firstName});
            _uut.Add(new User { FirstName = firstName});
            _uut.Add(new User ());
            SaveChanges();

            // Act
            var result = _uut.Find(x => x.FirstName == firstName);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void SingleOrDefault_NoEntityInDatabase_ReturnsNull()
        {
            // Arrange
            
            // Act
            var result = _uut.SingleOrDefault(x => x.FirstName == "hans");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SingleOrDefault_EntityInDatabase_ReturnsEntity()
        {
            // Arrange
            var user = new User{FirstName = "Hans"};
            _uut.Add(user);
            SaveChanges();

            // Act
            var result = _uut.SingleOrDefault(x => x.FirstName == user.FirstName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void SingleOrDefault_TwoEntitiesInDatabase_ReturnsNull()
        {
            // Arrange
            var user = new User { FirstName = "Hans" };
            _uut.Add(user);
            _uut.Add(user);
            SaveChanges();

            // Act
            var result = _uut.SingleOrDefault(x => x.FirstName == "hans");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddRange_AddEntities_EntitiesWasAdded()
        {
            // Arrange
            var users = new List<User>
            {
                new User(),
                new User()
            };
            
            // Act
            _uut.AddRange(users);
            SaveChanges();

            // Assert
            Assert.AreEqual(2, _uut.GetAll().Count());
        }

        [TestMethod]
        public void Update_EntityInDatabase_EntityWasUpdated()
        {
            // Arrange
            var user = new User();
            _uut.Add(user);
            SaveChanges();
            
            // Act
            user.FirstName = "Hans";
            _uut.Update(user);
            SaveChanges();

            // Assert
            var result = _uut.GetAll();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(user, result.First());
        }

        [TestMethod]
        public void Update_NoEntityInDatabase_EntityWasAdded()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Hans"
            };

            // Act
            _uut.Update(user);
            SaveChanges();

            // Assert
            var result = _uut.GetAll();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(user, result.First());
        }

        [TestMethod]
        public void Remove_EntityInDatabase_EntityIsRemoved()
        {
            // Arrange
            var user = new User();
            _uut.Add(user);
            SaveChanges();

            // Act
            _uut.Remove(user);
            SaveChanges();

            // Assert
            var result = _uut.GetAll();
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Remove_NoEntityInDatabase_ThrowExecpetion()
        {
            // Arrange
            var user = new User();
            
            // Act
            _uut.Remove(user);

            // Assert
            Assert.ThrowsException<DbUpdateConcurrencyException>(() => SaveChanges());
        }

        [TestMethod]
        public void RemoveRange_EntitiesInDatabase_EntitiesAreRemoved()
        {
            // Arrange
            var users = new List<User>
            {
                new User(),
                new User()
            };
            _uut.AddRange(users);
            SaveChanges();

            // Act
            _uut.RemoveRange(users);
            SaveChanges();

            // Assert
            Assert.AreEqual(0, _uut.GetAll().Count());
        }

        [TestMethod]
        public void RemoveRange_NoEntitiesInDatabase_ThrowException()
        {
            // Arrange
            var users = new List<User>
            {
                new User(),
                new User()
            };
            
            // Act
            _uut.RemoveRange(users);

            // Assert
            Assert.ThrowsException<DbUpdateConcurrencyException>(() => SaveChanges());
        }
    }
}
