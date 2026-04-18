using Shared.Common;
using Task.Api.Dtos;

namespace Task.Api.Services
{
    public interface IUserService
    {
        Task<Result<UserCreateReturnDto>> CreateUser(UserCreateInDto dto);

        Task<Result> UpdateUser(int userNumber,UserUpdateInDto dto);

        Task<Result> DelUser(int userNumber);
    }
}
