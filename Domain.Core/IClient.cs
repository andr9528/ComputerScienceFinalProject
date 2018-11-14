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
        ICollection<IClientAd> Ads { get; set; }
        TimeSpan AdsPlayTime { get; set; }
        int AdsPlayCount { get; set; }

        int AdsCount { get; }
    }
    
}
