using MapsterMapper;
using Shared.Common;
using Task.Api.Data;
using Task.Api.Dtos;
using Task.Api.Dtos.Project.In;
using Task.Api.Entities;

namespace Task.Api.Services
{
    public class ProjectService:IProjectService
    {
        private readonly IMapper _mapster;

        private readonly TaskDbContext _dbContext;

        public ProjectService(IMapper mapster, TaskDbContext taskDbContext)
        {
            _mapster = mapster;
            _dbContext = taskDbContext;
        }

        public async Task<Result> CreateProject(ProjectCreateInDto dto)
        {
            try
            {
                Projects projItem = _mapster.Map<Projects>(dto);
                projItem.Id = Guid.NewGuid();
                projItem.CreatedTime = DateTime.UtcNow;
                _dbContext.Projects.Add(projItem);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return Result.Success();
                }
                return Result.Failure("Insert Failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateProject(Guid projId, ProjectUpdateInDto dto)
        {
            try
            {
                Projects projItem = _dbContext.Projects.Find(projId);
                if (projItem == null)
                    return Result.Failure("Can't find this project");
                _mapster.Map(dto, projItem);
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

        public async Task<Result> DelProject(Guid projId)
        {
            try
            {
                Projects projItem = _dbContext.Projects.Find(projId);
                if (projItem == null)
                    return Result.Failure("Can't find this project");
                projItem.IsVaild = false;
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
