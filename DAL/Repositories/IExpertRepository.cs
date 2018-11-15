using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IExpertRepository : IRepository<Expert>
    {
        Expert GetByEmail(string email);
        Expert GetEager(Guid id);
        Expert GetEagerByEmail(string email);
        IQueryable<Expert> GetExpertsWithTagName(string tagName);
        IQueryable<Expert> GetAllEager();
    }
}
