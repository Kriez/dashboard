using kriez.HomeDashboard.Data.Configuration;
using kriez.HomeDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;

// dotnet ef --startup-project ../HomeDashboard/ migrations add Initial
namespace kriez.HomeDashboard.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base()
        { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HueScenesConfiguration());
            modelBuilder.ApplyConfiguration(new HueLightConfiguration());
            modelBuilder.ApplyConfiguration(new UpdateTableConfiguration());
        }

        public virtual DbSet<HueLight> HueLights { get; set; }
        public virtual DbSet<HueScene> HueScenes { get; set; }
        public virtual DbSet<CalendarItem> CalendarItems { get; set; }
        public virtual DbSet<UpdateTable> UpdateTables { get; set; }
    }
}
