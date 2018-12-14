using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;

namespace Domain.Concrete
{
    
    public class Client : IClient
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<IClientPlaylist> Playlists { get; set; }
        public TimeSpan AdsPlayTime { get; set; }
        public int AdsPlayCount { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
        public DateTime LastPing { get; set; }

        public Client()
        {
            Playlists = new List<IClientPlaylist>();
            CreationDate = DateTime.Now;
        }
    }
    
}
