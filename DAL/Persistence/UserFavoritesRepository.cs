using DAL.Data;
using DAL.Models.ManyToMany;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class UserFavoritesRepository : Repository<UserFavorites>, IUserFavoritesRepository
    {
        public UserFavoritesRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
    }
}
