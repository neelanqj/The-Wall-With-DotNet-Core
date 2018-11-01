using Microsoft.EntityFrameworkCore;
using Models;
using The_Wall_With_DotNet_Core.Models;

namespace Persistance
{
    public class MyDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
            
        }
    }
}