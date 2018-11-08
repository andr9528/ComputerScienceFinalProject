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
        string FilePath { get; set; }
        DateTime CreationDate { get; set; }
        string FileExtension { get; set; }
        ICollection<IClient> Clients { get; set; }
    }
    
}
