using AutoMapper;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Repository;

namespace WorkSphere.Server.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepo _repository;
        private readonly IMapper _mapper;

        public ProjectTaskService(IProjectTaskRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProjectTasksResponseDto> GetProjectTasksAsync(int projectId)
        {

            return await _repository.GetProjectTasksAsync(projectId);
        }

        public Task<List<ProjectTaskDto>> BulkUpdateProjectTasks(ProjectTaskBulkInsertDto projectTaskBulkInsertDto)
        {
            return _repository.BulkUpdateProjectTasks(projectTaskBulkInsertDto);
        }
    }
}
