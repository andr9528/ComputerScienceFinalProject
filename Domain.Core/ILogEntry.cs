using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface ILogEntry : IEntity
    {
        DateTime TimeStamp { get; set; }
        string Message { get; set; }
        string StackTrace { get; set; }
        ILogEntry InnerLogEntry { get; set; }
        int FK_LogEntry { get; set; }
        ILogEntry OuterLogEntry { get; set; }
    }
    
}
