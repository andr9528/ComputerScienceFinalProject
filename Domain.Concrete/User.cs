using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;

namespace Domain.Concrete
{

    public class User : IUser
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
    }

}
