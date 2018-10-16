using System.Linq;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
        public Tag GetTagByName(string tagName)
        {
            return OurContext.Tags.SingleOrDefault(x => x.Name == tagName);
        }
    }
}
