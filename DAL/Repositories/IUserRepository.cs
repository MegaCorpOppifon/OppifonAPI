using System;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);
        void RemoveById(Guid id);
    }
}
