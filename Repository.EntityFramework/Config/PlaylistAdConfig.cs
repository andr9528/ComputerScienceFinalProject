using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Repository.EntityFramework.Config
{

    
    public class PlaylistAdConfig : IEntityTypeConfiguration<PlaylistAd>
    {
        public PlaylistAdConfig()
        {
        }

        public void Configure(EntityTypeBuilder<PlaylistAd> builder)
        {
            // Defining Primary Key -->
            builder.HasKey(x => new {x.FK_AdId, x.FK_PlaylistId});

            // Defining Relations -->
            builder.HasOne(x => (Playlist) x.Playlist).WithMany(x => (ICollection<PlaylistAd>) x.Ads).HasForeignKey(x => x.FK_AdId);
            builder.HasOne(x => (Ad) x.Ad).WithMany(x => (ICollection<PlaylistAd>) x.Playlists).HasForeignKey(x => x.FK_PlaylistId);
        }
    }
    
}