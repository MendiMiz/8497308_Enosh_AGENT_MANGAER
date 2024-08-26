using WebApplication1.Dto;

namespace WebApplication1.Service
{
    public interface IMissionsSelectService
    {
        Task<List<MissionDto>> GetOrderedMissionProposal();
        Task ActivateMission(int missionId);
    }
}
