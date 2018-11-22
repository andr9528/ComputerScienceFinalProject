using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{

    public class PlaylistAd : IPlaylistAd
    {
        public int FK_AdId { get; set; }
        public virtual IAd Ad { get; set; }


        public int FK_PlaylistId { get; set; }
        public virtual IPlaylist Playlist { get; set; }
    }
}
