using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Core;
using Helpers;

namespace Domain.Core
{
    public interface IPlaylist : IEntity
    {


        ICollection<IClientPlaylist> Clients { get; set; }
        ICollection<IPlaylistAd> Ads { get; set; }
        DateTime CreationDate { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        Enums.PlayMethods PlayMethod { get; set; }

    }
}
