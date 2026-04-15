namespace Task.Api.Enum
{
    /// <summary>
    /// For table "TaskItem" "Status"
    /// </summary>
    public enum TaskStatusEnum
    {
        Created = 0,
        InProgressw = 1,
        PendingVerification = 2,
        Completed = 3,
        Rejected = 4,
        Canceled = 5,
        Deleted = 6
    }

}
