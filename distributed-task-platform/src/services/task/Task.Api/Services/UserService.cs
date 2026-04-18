using MapsterMapper;
using Shared.Common;
using Task.Api.Data;
using Task.Api.Dtos;
using Task.Api.Entities;

namespace Task.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapster;

        private readonly TaskDbContext _dbContext;

        public UserService(IMapper mapster, TaskDbContext taskDbContext) 
        { 
            _mapster = mapster;
            _dbContext = taskDbContext;
        }

        public async Task<Result<UserCreateReturnDto>> CreateUser(UserCreateInDto userCreateInDto)
        {
            try
            {
                Users userDataItem = _mapster.Map<Users>(userCreateInDto);
                userDataItem.Id = Guid.NewGuid();
                userDataItem.CreatedTime = DateTime.UtcNow;
                _dbContext.Users.Add(userDataItem);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    UserCreateReturnDto returnDto = _mapster.Map<UserCreateReturnDto>(userDataItem);
                    return Result<UserCreateReturnDto>.Success(returnDto);
                }
                return Result<UserCreateReturnDto>.Failure("Insert Failed");
            }
            catch (Exception ex)
            {
                return Result<UserCreateReturnDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateUser(int userNumber, UserUpdateInDto dto)
        {
            try
            {
                Users userDataItem = _dbContext.Users.Where(x => x.UserNumber == userNumber).FirstOrDefault();
                if (userDataItem == default)
                    return Result.Failure("Can't find this user");
                _mapster.Map(dto, userDataItem);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return Result.Success();
                }
                return Result.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DelUser(int userNumber)
        {
            try
            {
                Users userDataItem = _dbContext.Users.Where(x => x.UserNumber == userNumber).FirstOrDefault();
                if (userDataItem == default)
                    return Result.Failure("Can't find this user");
                userDataItem.IsVaild = false;
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return Result.Success();
                }
                return Result.Failure("Delete failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
