using kriez.HomeDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kriez.HomeDashboard.Data.Configuration
{
    public class HueLightConfiguration : IEntityTypeConfiguration<HueLight>
    {
        public void Configure(EntityTypeBuilder<HueLight> modelBuilder)
        {
            modelBuilder
                .HasKey(l => l.Id);

            modelBuilder
                .Property(l => l.Id)
                .ValueGeneratedNever();

            modelBuilder
               .HasOne<HueScene>()
               .WithMany(s => s.Lights)
               .HasForeignKey(o => o.Group)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
