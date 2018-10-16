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
    }
}
