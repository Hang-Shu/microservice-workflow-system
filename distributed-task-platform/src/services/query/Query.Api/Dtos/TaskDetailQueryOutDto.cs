namespace Query.Api.Dtos
{
    public class TaskDetailQueryOutTimeLineDto
    {
        public int TotalComments { get; set; }

        public int TotalEmails { get; set; }

        public List<ActiveDto> lstActives { get; set; } = new();
    }
}
