using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DAL.Persistence
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRepository(DbContext context, IPasswordHasher<User> passwordHasher) : base(context)
        {
            _passwordHasher = passwordHasher;
        }

        public User GetByEmail(string email)
        {
           return OurContext.Users.FirstOrDefault(u => u.UserName == email);
        }
        
        public void RemoveById(Guid id)
        {
            var user = OurContext.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
                return;

            OurContext.Users.Remove(user);
        }


        public Context OurContext => Context as Context;
      
    }
}
