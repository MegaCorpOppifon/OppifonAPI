using System;
using System.Collections.Generic;
using System.Linq;
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
        public ICollection<UserFavorites> GetFavorites(Guid userId)
        {
            var dbUser = OurContext.Users.Where(x => x.Id == userId)
                .Include(x => x.Favorites)
                .ThenInclude(x => x.Expert)
                .SingleOrDefault();

            return dbUser?.Favorites.Where(x => x.UserId == userId).ToList();
        }
    }
}
