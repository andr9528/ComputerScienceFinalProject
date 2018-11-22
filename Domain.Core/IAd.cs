using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    
    public interface IAd : IEntity
    {
        string Name { get; set; }
        string FileLocation { get; set; }
        DateTime CreationDate { get; set; }
        string FileExtension { get; set; }
        ICollection<IPlaylistAd> Playlists { get; set; }
        TimeSpan TotalPlayTime { get; set; }
        int TotalPlayCount { get; set; }
        string Description { get; set; }

        string CompleteFilePath { get; }
    }
    
}
