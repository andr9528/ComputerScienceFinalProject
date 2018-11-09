using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntityFramework.Config
{

   
    public class AdConfig : IEntityTypeConfiguration<Ad>
    {
        public AdConfig()
        {
        }

        public void Configure(EntityTypeBuilder<Ad> builder)
        {
            // Defining Primary Key -->
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("AdId");

            // Should theoretically set the time the entity is created when it is added to the database
            builder.Property(x => x.CreationDate).ValueGeneratedOnAdd();
            builder.Property(x => x.CreationDate).HasDefaultValue(DateTime.Now);

            // Defining Properties that are not stored in database -->
            builder.Ignore(x => x.CompleteFilePath).Ignore(x=>x.ClientsCount);
        }
    }
    
}