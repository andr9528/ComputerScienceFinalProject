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
        public string AdName { get; set; }
        public string FileLocation { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileExtension { get; set; }
        public virtual ICollection<IClientAd> Clients { get; set; }
        public int Id { get; set; }
        public TimeSpan TotalPlayTime { get; set; }
        public int TotalPlayCount { get; set; }

        public string CompleteFilePath { get { return string.Format("{0}{1}{2}", FileLocation, AdName, FileExtension); } }
        public int ClientsCount { get { return Clients.Count; } }

        public Ad()
        {
            Clients = new List<IClientAd>();
        }
    }
}
