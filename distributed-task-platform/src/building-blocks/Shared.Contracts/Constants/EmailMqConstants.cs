using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class EmailMqConstants
    {
        public const string Exchange = "email-events";
        public static class RoutingKeys
        {
            public const string EmailSend = "email.send";
        }
    }
}
