using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Services
{
    public interface ITeamService
    {
        public Task<PagedTeamResponseDto> GetPagedTeams(int pageIndex, int pageSize);

        public Task<TeamDto> GetTeamById(int teamId);

        public Task<TeamDto> UpdateTeamAsync(UpdateTeamDto updateTeamDto);
    }
}
