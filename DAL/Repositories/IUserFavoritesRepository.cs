using System;
using System.Collections.Generic;
using DAL.Models.ManyToMany;

namespace DAL.Repositories
{
    public interface IUserFavoritesRepository : IRepository<UserFavorites>
    {
        ICollection<UserFavorites> GetFavorites(Guid userId);
    }
}
