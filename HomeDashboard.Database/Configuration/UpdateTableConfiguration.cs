using kriez.HomeDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kriez.HomeDashboard.Data.Configuration
{
    public class UpdateTableConfiguration : IEntityTypeConfiguration<UpdateTable>
    {
        public void Configure(EntityTypeBuilder<UpdateTable> modelBuilder)
        {
            modelBuilder
                .HasKey(l => l.Key);

            modelBuilder
                .Property(l => l.Key)
                .ValueGeneratedNever();
        }
    }
}
