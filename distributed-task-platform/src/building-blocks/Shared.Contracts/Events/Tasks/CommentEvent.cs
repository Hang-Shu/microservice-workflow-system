using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public class CommentEvent
    {
        public Guid CommentId { get; set; }
        public int TaskNumber { get; set; }
        public int UserNumber { get; set; }
        public string DisplayNameSnapshot { get; set; }
        public string CommentText { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
