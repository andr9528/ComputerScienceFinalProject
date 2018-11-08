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
        public virtual ICollection<IClientAd> Ads { get; set; }
        public TimeSpan AdsPlayTtime { get; set; }
        public int AdsPlayCount { get; set; }


        public int AdsCount
        {
            get { return Ads.Count; }
        }

        public Client()
        {
            Ads = new List<IClientAd>();
        }
    }
    
}
