using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public class EmailSendEvent
    {
        public Guid EmailId { get; set; }
        public List<int> TaskNumbers { get; set; }
        public int ReciveUserNumber { get; set; }
        public string DisplayNameSnapshot { get; set; }
        public string StatusName { get; set; }
        public string Reason { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
