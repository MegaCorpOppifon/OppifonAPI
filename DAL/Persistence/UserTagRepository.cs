using DAL.Data;
using DAL.Models.ManyToMany;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class UserTagRepository : Repository<UserTag>, IUserTagRepository
    {
        public UserTagRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
    }
}
