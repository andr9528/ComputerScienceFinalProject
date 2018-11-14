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
        string AdName { get; set; }
        string FileLocation { get; set; }
        DateTime CreationDate { get; set; }
        string FileExtension { get; set; }
        ICollection<IClientAd> Clients { get; set; }
        TimeSpan TotalPlayTime { get; set; }
        int TotalPlayCount { get; set; }

        string CompleteFilePath { get; }
        int ClientsCount { get; }
    }
    
}
