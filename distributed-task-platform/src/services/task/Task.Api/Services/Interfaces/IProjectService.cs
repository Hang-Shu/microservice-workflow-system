using Shared.Common;
using Task.Api.Dtos;
using Task.Api.Dtos.Project.In;

namespace Task.Api.Services
{
    public interface IProjectService
    {
        Task<Result> CreateProject(ProjectCreateInDto dto);

        Task<Result> UpdateProject(Guid projId, ProjectUpdateInDto dto);

        Task<Result> DelProject(Guid projId);
    }
}
