using kriez.HomeDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kriez.HomeDashboard.Data.Configuration
{
    public class CalendarItemConfiguration : IEntityTypeConfiguration<CalendarItem>
    {
        public void Configure(EntityTypeBuilder<CalendarItem> modelBuilder)
        {
            modelBuilder
                .HasKey(l => l.Id);

            modelBuilder
                .Property(l => l.Id)
                .ValueGeneratedNever();
        }
    }
}
