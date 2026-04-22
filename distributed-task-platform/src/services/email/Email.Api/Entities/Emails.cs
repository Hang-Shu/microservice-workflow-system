using Email.Api.Enum;
using System.ComponentModel.DataAnnotations;

namespace Email.Api.Entities
{
    //Record all Email send
    public class Emails
    {
        [Key]
        public Guid Id { get; set; }

        public int ReciveUserNumber { get; set; }

        [Required]
        public string EmailAccount {  get; set; }

        [Required]
        public string EmailTitle { get; set; }

        [Required]
        public string EmailText  { get; set; }

        [Required]
        public EmailStatusEnum EmailStatus { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? SentTime { get; set; }

        public string? LastErrorMsg { get; set; }


    }
}
