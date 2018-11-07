using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IUser : IEntity
    {
        string Email { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
    
}
