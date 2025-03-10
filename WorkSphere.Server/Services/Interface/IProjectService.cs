﻿using WorkSphere.Model;
using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Services
{
    public interface IProjectService
    {

        public Task<PagedProjectsResponseDto> GetPagedProjectsAsync(int pageIndex, int pageSize);
        public Task<int> GetTotalProjectCountAsync();

        public Task<Project> GetProjectAsync(int projectId);

        public Task<Project> CreateProjectAsync(Project project);

        public Task<Project> UpdateProjectAsync(int id, Project project);

        public Task<Project> DeleteProjectAsync(int projectId);

    }
}
