using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;

namespace Domain.Concrete
{
    public class ClientPlaylist : IClientPlaylist
    {
        public int FK_PlaylistId { get; set; }
        public virtual IPlaylist Playlist { get; set; }

        public int FK_ClientId { get; set; }
        public virtual IClient Client { get; set; }
    }
}
