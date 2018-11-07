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
        Exception Exception { get; set; }
        string StackTrace { get; set; }
    }
    
}
