namespace Task.Api.Dtos.Project.In
{
    public class ProjectUpdateInDto
    {
        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public string ProjectVersion { get; set; } = "V1.0";

        public string? Remark { get; set; }
    }
}
