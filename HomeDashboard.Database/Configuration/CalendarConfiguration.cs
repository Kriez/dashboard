using kriez.HomeDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kriez.HomeDashboard.Data.Configuration
{
    public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> modelBuilder)
        {
            modelBuilder
                .HasKey(l => l.Id);

            modelBuilder
                .Property(l => l.Id)
                .ValueGeneratedNever();

        }
    }
}
