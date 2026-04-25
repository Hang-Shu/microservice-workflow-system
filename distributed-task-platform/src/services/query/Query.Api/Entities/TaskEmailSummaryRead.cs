using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Query.Api.Entities
{
    public class TaskEmailSummaryRead
    {
        [Key]
        public Guid Id { get; set; }

        public int TaskNumber { get; set; }

        //Connect "Emails"-"Id"
        public Guid EmailId { get; set; }

        //unique
        public string IdempotentId { get; set; }

        public int ReciveUserNumber { get; set; }

        [Required]
        public string DisplayNameSnapshot { get; set; }

        [Required]
        public string StatusName { get; set; }

        public string Reason { get; set; }

        public DateTime CreateTime { get; set; }


    }
}
