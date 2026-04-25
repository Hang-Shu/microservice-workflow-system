using Shared.Common;
using Shared.Contracts.Events;

namespace Query.Api.Services
{
    public interface IEmailSendService
    {
        public Task<Result> HandleEmailSendAsync(EmailSendEvent dto);
    }
}
