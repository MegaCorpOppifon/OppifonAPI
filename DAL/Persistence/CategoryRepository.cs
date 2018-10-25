using System;
using System.Linq;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
        public Category GetCategoryByName(string categoryName)
        {
            return OurContext.Categories.SingleOrDefault(x => x.Name == categoryName);
        }

        public Category GetCategoryEagerByName(string categoryName)
        {
            return OurContext.Categories.Where(x => string.Compare(x.Name, categoryName, StringComparison.InvariantCultureIgnoreCase) == 0)
                .Include(x => x.Tags)
                .Include(x => x.Experts)
                .ThenInclude(x => x.MainFields)
                .ThenInclude(x => x.Tag)
                .Include(x => x.Experts)
                .ThenInclude(x => x.ExpertTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.Experts)
                .ThenInclude(x => x.ExpertCategory)
                .SingleOrDefault();
        }
    }
}
