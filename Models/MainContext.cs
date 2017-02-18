using Microsoft.EntityFrameworkCore;

namespace blackBelt.Models
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        { }
        public DbSet<User> Users {get; set;}
        public DbSet<Auction> Auctions {get; set;}
    }
}