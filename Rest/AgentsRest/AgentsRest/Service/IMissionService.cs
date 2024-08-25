using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> ActualMissionProposalWhenAgentMove(int agentId);
        Task<List<MissionModel>> ActualMissionProposalWhenTargetMove(int targetId);
        Task UpdateMissions();
        Task ActivateMissionById(int missionId);

    }
}
