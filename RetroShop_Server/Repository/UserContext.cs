using Microsoft.EntityFrameworkCore;

namespace RetroShop_Server.Repository
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Model.User> User { get; set; }
    }
}
