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
        public string FilePath { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileExtension { get; set; }
        public ICollection<IClient> Clients { get; set; }
        public int Id { get; set; }
    }
}
