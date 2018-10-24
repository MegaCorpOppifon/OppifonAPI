using DAL.Models;

namespace DAL.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetCategoryByName(string categoryName);
        Category GetCategoryEagerByName(string categoryName);
    }
}
