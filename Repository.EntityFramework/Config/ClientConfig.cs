using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Repository.EntityFramework.Config
{


    public class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public ClientConfig()
        {
        }

        public void Configure(EntityTypeBuilder<Client> builder)
        {
            // Defining Primary Key -->
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ClientId");

            // Should theoretically set the time the entity is created when it is added to the database
            builder.Property(x => x.CreationDate).ValueGeneratedOnAdd();
            builder.Property(x => x.CreationDate).HasDefaultValue(DateTime.Now);

            // Defining Properties that are not stored in database -->
            builder.Ignore(x => x.AdsCount);

        }
    }
    
}