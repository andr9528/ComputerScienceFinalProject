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

            // Defining that a property need to be unique in the database -->
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
    
}