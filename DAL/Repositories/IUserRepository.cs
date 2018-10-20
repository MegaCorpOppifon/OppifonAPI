using System;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);
        User GetEagerByEmail(string email);
        User GetEager(Guid id);
        void RemoveById(Guid id);
    }
}
