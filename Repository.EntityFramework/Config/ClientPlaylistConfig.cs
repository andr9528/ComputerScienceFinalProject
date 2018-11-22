using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntityFramework.Config
{
    public class ClientPlaylistConfig : IEntityTypeConfiguration<ClientPlaylist>
    {
        public ClientPlaylistConfig()
        {
        }

        public void Configure(EntityTypeBuilder<ClientPlaylist> builder)
        {
            // Defining Primary Key -->
            builder.HasKey(x => new { x.FK_ClientId, x.FK_PlaylistId });

            // Defining Relations -->
            builder.HasOne(x => (Playlist)x.Playlist).WithMany(x => (ICollection<ClientPlaylist>)x.Clients).HasForeignKey(x => x.FK_ClientId);
            builder.HasOne(x => (Client)x.Client).WithMany(x => (ICollection<ClientPlaylist>)x.Playlists).HasForeignKey(x => x.FK_PlaylistId);
        }
    }
}
