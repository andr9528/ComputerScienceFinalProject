using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntityFramework.Config
{

    public class LogEntryConfig : IEntityTypeConfiguration<LogEntry>
    {
        public LogEntryConfig()
        {
        }

        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            // Defining Primary Key -->
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("LogEntryId");
        }
    }
    
}