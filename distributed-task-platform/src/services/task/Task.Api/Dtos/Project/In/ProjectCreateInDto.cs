namespace Task.Api.Dtos
{
    public class ProjectCreateInDto
    {
        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public string ProjectVersion { get; set; } = "V1.0";

        public string? Remark { get; set; }

        public int CreatedUserNumber { get; set; }
    }
}
