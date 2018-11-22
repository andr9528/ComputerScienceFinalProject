using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IClientPlaylist
    {
        int FK_PlaylistId { get; set; }
        IPlaylist Playlist { get; set; }

        int FK_ClientId { get; set; }
        IClient Client { get; set; }
    }
}
