﻿using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> ActualMissionProposalWhenAgentMove(int agentId);
        Task<List<MissionModel>> ActualMissionProposalWhenTargetMove(TargetModel target);

    }
}
