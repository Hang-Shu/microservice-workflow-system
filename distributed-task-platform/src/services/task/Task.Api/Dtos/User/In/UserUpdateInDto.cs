namespace Task.Api.Dtos
{
    public class UserUpdateInDto
    {
        public string UserName { get; set; }

        public string? DisplayName { get; set; }

        public string? Department { get; set; }

        public string? UserPhone { get; set; }

        public string? UserEmail { get; set; }

        public DateTime? JoinTime { get; set; }

        public string? Remark { get; set; }

    }
}
