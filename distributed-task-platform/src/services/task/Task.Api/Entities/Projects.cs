using System.ComponentModel.DataAnnotations;

namespace Task.Api.Entities
{
    public class Projects
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProjectName { get; set; }

        [Required]
        [MaxLength(500)]
        public string ProjectDescription { get; set; }

        [MaxLength(100)]
        public string ProjectVersion { get; set; } = "V1.0";

        public string? Remark { get; set; }

        public int CreatedUserNumber { get; set; }

        public bool IsVaild { get; set; } = true;

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
