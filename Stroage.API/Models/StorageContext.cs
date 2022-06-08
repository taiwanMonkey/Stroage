using Microsoft.EntityFrameworkCore;

namespace Stroage.API.Models
{
    public class StorageContext : DbContext
    {

        public StorageContext(DbContextOptions<StorageContext> options) 
            : base(options){}

        public DbSet<Material> Materials { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Pack> Pack { get; set; }
        public DbSet<Storehouse> Storehouses { get; set; }
        public DbSet<Bin> Bins { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
    }
}
