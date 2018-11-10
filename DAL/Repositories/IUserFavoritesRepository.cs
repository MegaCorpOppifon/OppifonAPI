using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models.ManyToMany;

namespace DAL.Repositories
{
    public interface IUserFavoritesRepository : IRepository<UserFavorites>
    {
        IQueryable<UserFavorites> GetFavorites(Guid userId);
    }
}
