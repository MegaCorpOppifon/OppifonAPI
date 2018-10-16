using DAL.Models;

namespace DAL.Repositories
{
    public interface IExpertRepository : IRepository<Expert>
    {
        Expert GetByEmail(string email);
    }
}
