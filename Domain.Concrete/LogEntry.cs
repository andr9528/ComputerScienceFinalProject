using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{

    public class LogEntry : ILogEntry
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string StackTrace { get; set; }
        public int Id { get; set; }
    }

}
