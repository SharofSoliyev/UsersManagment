using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagment.Core.Entities;

namespace UsersManagment.Infostructure.Data
{
   public class AppDataContext : DbContext
    {
     
        public AppDataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

    }
}
