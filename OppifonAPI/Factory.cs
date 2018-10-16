using DAL.Data;
using DAL.Factory;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace OppifonAPI
{
    public class Factory : IFactory
    {
        public static string ConnectionString { get; set; }
        private static Factory _instance;

        public static Factory Instance => _instance ?? (_instance = new Factory());

        public IUnityOfWork GetUOF()
        {
            return new UnitOfWork(new Context(ConnectionString), new PasswordHasher<User>());
        }
    }
}
