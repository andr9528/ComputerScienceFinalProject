using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IClientAd
    {
        int FK_AdId { get; set; }
        IAd Ad { get; set; }

        int FK_ClientId { get; set; }
        IClient Client { get; set; }
    }
}
