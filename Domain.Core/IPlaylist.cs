using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IPlaylist
    {
        ICollection<IClientPlaylist> Clients { get; set; }
        ICollection<IPlaylistAd> Ads { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        DateTime CreationDate { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
