using kriez.HomeDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kriez.HomeDashboard.Data.Configuration
{
    class HueScenesConfiguration : IEntityTypeConfiguration<HueScene>
    {
        public void Configure(EntityTypeBuilder<HueScene> modelBuilder)
        {
            modelBuilder
                .HasKey(l => l.Id);

            modelBuilder
                .Property(l => l.Id)
                .ValueGeneratedNever();
        }
    }
}
