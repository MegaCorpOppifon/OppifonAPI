using System.Collections.Generic;
using DAL.Models;

namespace DAL.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Tag GetTagByName(string tagName);
    }
}
