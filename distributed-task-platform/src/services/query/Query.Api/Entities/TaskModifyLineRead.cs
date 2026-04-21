using System.ComponentModel.DataAnnotations;

namespace Query.Api.Entities
{
    public class TaskModifyLineRead
    {
        [Key]
        public Guid Id { get; set; }

        public int TaskNumber { get; set; }

        public int ModifyUserNumber { get; set; }

        [Required]
        public string DisplayNameSnapshot { get; set; }

        public List<string> ListUpdateItems { get; set; } = new();

        public DateTime GroupStartTime { get; set; }

        public DateTime GroupEndTime { get; set; }
    }
}
