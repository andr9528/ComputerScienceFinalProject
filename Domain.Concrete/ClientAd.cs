using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{

    public class ClientAd : IClientAd
    {
        public int FK_AdId { get; set; }
        public IAd Ad { get; set; }

        public int FK_ClientId { get; set; }
        public IClient Client { get; set; }
    }
}
