using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Helpers;

namespace Domain.Concrete
{
    public class Playlist : IPlaylist
    {


        public virtual ICollection<IClientPlaylist> Clients { get; set; }
        public virtual ICollection<IPlaylistAd> Ads { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public Enums.PlayMethods PlayMethod { get; set; }

        public Playlist()
        {
            Clients = new List<IClientPlaylist>();
            Ads = new List<IPlaylistAd>();
            CreationDate = DateTime.Now;
        }
    }
}
