using System;
using System.Collections.Generic;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IExpertRepository : IRepository<Expert>
    {
        Expert GetByEmail(string email);
        Expert GetEager(Guid id);
        Expert GetEagerByEmail(string email);
        ICollection<Expert> GetExpertsWithTagName(string tagName);
    }
}
