using WorkSphere.Server.Dtos;
using WorkSphere.Server.Repository;

namespace WorkSphere.Server.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepo _teamRepo;

        public TeamService(ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public async Task<PagedTeamResponseDto> GetPagedTeams(int pageIndex, int pageSize)
        {
            return await _teamRepo.GetPagedTeams(pageIndex, pageSize);
        }

        public async Task<TeamDto> GetTeamById(int teamId)
        {
            return await _teamRepo.GetTeamById(teamId);
        }

        public async Task<TeamDto> UpdateTeamAsync(UpdateTeamDto updateTeamDto)
        {
            return await _teamRepo.UpdateTeamAsync(updateTeamDto);
        }

    }
}
