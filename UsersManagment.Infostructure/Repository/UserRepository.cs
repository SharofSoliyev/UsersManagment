using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagment.Core.Entities;
using UsersManagment.Infostructure.Data;

namespace UsersManagment.Infostructure.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUser(string username, string password);
        Task<User> CreateUser(User user);
    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDataContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> CreateUser(User user)
        {
            var newUser = await AddAsync(user);

            return newUser;
        }

        public async Task<User> GetUser(string username, string password)
        {
            return await GetAllByExp(s => s.UserName == username && s.Password == password).FirstOrDefaultAsync();
        }
    }
}
