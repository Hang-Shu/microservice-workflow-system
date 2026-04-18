using MapsterMapper;
using Notification.Api.Data;
using Notification.Api.Dtos;
using Notification.Api.Entities;
using Notification.Api.Enum;
using Shared.Common;
using Shared.Contracts.Events;

namespace Notification.Api.Services
{
    public class NotificationService : INotificationService
    {

        private readonly NotificationDbContext _dbContext;
        private readonly IMapper _mapster;
        public NotificationService(NotificationDbContext dbContext,IMapper mapster)
        {
            _dbContext = dbContext;
            _mapster = mapster;
        }

        public async Task<Result> CreateNewMessage(NotificationCreateInDto dto)
        {
            try
            {
                Notifications notiItem = _mapster.Map<Notifications>(dto);
                notiItem.Id = Guid.NewGuid();
                notiItem.CreateTime = DateTime.UtcNow;
                notiItem.NotificationCategory = NotificationCategoryEnum.People;
                List<Notifications_Dtl> lstDtls = new();

                foreach (int iReciveUserNumber in dto.lstReciveUserNumber)
                {
                    Notifications_Dtl dtls = new Notifications_Dtl()
                    {
                        Id = Guid.NewGuid(),
                        MainId = notiItem.Id,
                        ReciveUserNumber = iReciveUserNumber,
                        SendTime= notiItem.CreateTime
                    };
                    lstDtls.Add(dtls);
                }
                _dbContext.Notifications.Add(notiItem);
                _dbContext.Notifications_Dtl.AddRange(lstDtls);

                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                return Result.Failure("Create notification failed. Check data please.");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> HandleTaskCreatedAsync(TaskCreatedEvent taskEvent)
        {
            try
            {
                //Make idempotent key and search in NotificationInbox
                //idempotent key format: "{Event name}:{TaskNumber}/{AssignedUserNumber}/{Event time}"
                string strIdempotentKey = "TaskAssign:"+ taskEvent.TaskNumber.ToString()+"/"+taskEvent.AssignedUserNumber.ToString()+"/"+ taskEvent.CreatedTime.ToString("O");

                var inboxResult = _dbContext.NotificationInbox.Find(strIdempotentKey);
                if(inboxResult != null)     //Repeated message, return and ack
                    return Result.Success();

                string strMsgTitle = string.Format("{0}You got a new pending matter.",
                    string.IsNullOrWhiteSpace(taskEvent.ProjectName) ?
                    "" : "[" + taskEvent.ProjectName + "] ");

                string strMsgDescription = 
                    string.Format("You got a new task #{0:D4}", taskEvent.TaskNumber);

                Notifications notiItem = new Notifications()
                {
                    Id = Guid.NewGuid(),
                    MsgTitle= strMsgTitle,
                    MsgText = strMsgDescription,
                    IsSystemMsg=true,
                    CreateTime= taskEvent.CreatedTime
                };

                Notifications_Dtl dtls = new Notifications_Dtl()
                {
                    Id = Guid.NewGuid(),
                    MainId = notiItem.Id,
                    ReciveUserNumber = taskEvent.AssignedUserNumber.Value,
                    SendTime = DateTime.UtcNow
                };

                NotificationInbox inboxItem = new NotificationInbox()
                {
                    EventKey = strIdempotentKey,
                    CreateTime = DateTime.UtcNow
                };

                _dbContext.Notifications.Add(notiItem);
                _dbContext.Notifications_Dtl.Add(dtls);
                _dbContext.NotificationInbox.Add(inboxItem);

                if (await _dbContext.SaveChangesAsync() > 0)
                    return Result.Success();
                return Result.Failure("Create notification failed. Check data please.");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
