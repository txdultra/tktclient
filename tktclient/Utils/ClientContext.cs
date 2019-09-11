using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tktclient.Services;

namespace tktclient.Utils
{
    public class ClientContext
    {
        public static ClientUserDto CurrentUser { get; set; }
    }
}
