using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IPlaylistAd
    {
        int FK_AdId { get; set; }
        IAd Ad { get; set; }

        int FK_PlaylistId { get; set; }
        IPlaylist Playlist { get; set; }
    }
}
