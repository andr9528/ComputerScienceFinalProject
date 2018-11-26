using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityFramework.Config
{
    public class PlaylistConfig : IEntityTypeConfiguration<Playlist>
    {
        public PlaylistConfig()
        {
        }

        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            // Defining Primary Key -->
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PlaylistId");

            // Defining that a property need to be unique in the database -->
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
