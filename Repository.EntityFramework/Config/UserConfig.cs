﻿using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntityFramework.Config
{

    
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public UserConfig()
        {
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Defining Primary Key -->
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("UserId");
        }
    }
    
}