using System.ComponentModel.DataAnnotations;

namespace Query.Api.Entities
{
    public class TaskModifyLineRead
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }

        //Connect "TaskOperates"-"Id"
        public Guid TaskOperatesId { get; set; }

        public int ModifyUserNumber { get; set; }

        [Required]
        public string DisplayNameSnapshot { get; set; }

        [Required]
        public string PreviewText {  get; set; }

        public DateTime CreateTime { get; set; }
    }
}
