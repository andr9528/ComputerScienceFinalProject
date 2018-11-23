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

            // Defining a Zero-Or-One-To-One relation -->
            //builder.Property(x => x.InnerLogEntry).IsRequired(false);
            //builder.Property(x => x.OuterLogEntry).IsRequired(false);
            //builder.HasOne(x => (LogEntry) x.InnerLogEntry).WithOne(x => (LogEntry) x.OuterLogEntry)
            //    .HasForeignKey<LogEntry>(x => x.FK_LogEntry);
        }
    }
    
}