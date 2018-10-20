using System;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IExpertRepository : IRepository<Expert>
    {
        Expert GetByEmail(string email);
        Expert GetEager(Guid id);
        Expert GetEagerByEmail(string email);
    }
}
