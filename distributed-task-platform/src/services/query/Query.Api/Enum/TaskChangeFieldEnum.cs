using System.ComponentModel.DataAnnotations;

namespace Query.Api.Enum
{
    public enum TaskChangeFieldEnum
    {
        [Display(Name = "Title")]
        Title,
        [Display(Name = "Description")]
        Description,
        [Display(Name = "Project")]
        Project,
        [Display(Name = "Status")]
        Status,
        [Display(Name = "Priority")]
        Priority,
        [Display(Name = "Assigned User")]
        Assigned,
        [Display(Name = "Remark")]
        Remark,
        [Display(Name = "DueDate")]
        DueDate
    }
}
