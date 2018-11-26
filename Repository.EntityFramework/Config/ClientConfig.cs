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

            // Defining that a property need to be unique in the database -->
            builder.HasIndex(x => x.Ip).IsUnique();
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
    
}