using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;

namespace Domain.Concrete
{
    public class Ad : IAd
    {
        public string Name { get; set; }
        public string FileLocation { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileExtension { get; set; }
        public virtual ICollection<IPlaylistAd> Playlists { get; set; }
        public int Id { get; set; }
        public TimeSpan TotalPlayTime { get; set; }
        public int TotalPlayCount { get; set; }
        public string Description { get; set; }

        public string CompleteFilePath { get { return string.Format(@"{0}\{1}{2}", FileLocation, Name, FileExtension); } }

        public Ad()
        {
            Playlists = new List<IPlaylistAd>();
            FileLocation = @"C:\inetpub\wwwroot\AdProgram\Ads";
            CreationDate = DateTime.Now;
        }
    }
}
