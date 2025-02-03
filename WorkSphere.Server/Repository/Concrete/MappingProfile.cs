using AutoMapper;
using WorkSphere.Model;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository.Concrete
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectTaskDto, ProjectTask>();

            // Map ProjectTask to ProjectTaskDto
            CreateMap<ProjectTask, ProjectTaskDto>();
        }
    }
}
