using System.Linq;
using DAL.Data;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest_DAL
{
    [TestClass]
    public class UnitOfWorkTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "Oppifon")
                .Options;

            using (var unit = new UnitOfWork(new Context(options)))
            {
                unit.Users.Add(new User());
                unit.Complete();

                Assert.AreEqual(1, unit.Users.GetAll().Count());
            }
        }
    }
}
