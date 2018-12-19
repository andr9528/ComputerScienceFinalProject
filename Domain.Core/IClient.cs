using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IClient : IEntity
    {
        string Ip { get; set; }
        DateTime CreationDate { get; set; }
        ICollection<IClientPlaylist> Playlists { get; set; }
        TimeSpan AdsPlayTime { get; set; }
        int AdsPlayCount { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool State { get; set; }
        DateTime LastPing { get; set; }

    }
    
}
