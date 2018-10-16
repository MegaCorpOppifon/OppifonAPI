using DAL.Data;
using DAL.Models.ManyToMany;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class MainFieldTagRepository : Repository<MainFieldTag>, IMainFieldTagRepository
    {
        public MainFieldTagRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
    }
}
