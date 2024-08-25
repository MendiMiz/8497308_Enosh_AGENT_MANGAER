using AgentsRest.Data;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgentsRest.Service
{
    public class MissionService(ApplicationDbContext context) : IMissionService
    {

        public async Task<List<MissionModel>> ActualMissionProposalWhenAgentMove(int agentId)
        {
            AgentModel? agent = await context.Agents.FindAsync(agentId);
            if (agent == null) { throw new Exception($"failed to find a user with {agentId} id"); }

            List<int> targetsInMissionIds = await context.Missions
                .Where(m => m.Status != MissionStatus.Proposal)
                .Select(m => m.TargetId).ToListAsync();
            List<int> targetsInMissionForAgent =
                 await context.Missions
                 .Where(m => m.AgentId == agentId)
                 .Select(m => m.TargetId).ToListAsync();
            List<TargetModel> targets = await context.Targets.ToListAsync();

            foreach (TargetModel target in targets)
            {
                //if target not exists in a mission already
                if (!targetsInMissionIds.Any(tId => tId == target.Id))
                    if (!targetsInMissionForAgent.Any(tId => tId == target.Id))
                    {
                        double AgentToTargetDistance = Distance(agent, target);
                        if (AgentToTargetDistance <= 200)
                        {
                            await CreateMission(agent.Id, target.Id, AgentToTargetDistance);
                        }
                    }
            }
            return await context.Missions.Where(m => m.Status == MissionStatus.Proposal).ToListAsync();
        }

        public async Task<List<MissionModel>> ActualMissionProposalWhenTargetMove(TargetModel targetId)
        {
            TargetModel? target = await context.Targets.FindAsync(targetId);
            if (target == null) { throw new Exception($"failed to find a user with {targetId} id"); }

            if (target.Status != StatusTarget.InProgress)
            {
                List<AgentModel> inactiveAgents = await context.Agents
                    .Where(agent => agent.Status == StatusAgent.Inactive).ToListAsync();

                List<int> targetsInMissionIds = await context.Missions
               .Where(m => m.Status != MissionStatus.Proposal)
               .Select(m => m.TargetId).ToListAsync();

                List<int> targetsInMissionForTarget =
                await context.Missions
                 .Where(m => m.TargetId == target.Id)
                 .Select(m => m.AgentId).ToListAsync();

                if (!targetsInMissionIds.Any(tId => tId == target.Id))
                {
                    foreach (AgentModel agent in inactiveAgents)
                    {
                        if (!targetsInMissionForTarget.Any(tId => tId == target.Id))
                        {
                            double AgentToTargetDistance = Distance(agent, target);
                            if (AgentToTargetDistance <= 200)
                            {
                                await CreateMission(agent.Id, target.Id, AgentToTargetDistance);
                            }
                        }
                    }
                }
            }
            return await context.Missions.Where(m => m.Status == MissionStatus.Proposal).ToListAsync();
        }

        public async Task CreateMission(int agentId, int targetId, double distance)
        {
            MissionModel newMission = new()
            {
                AgentId = agentId,
                TargetId = targetId,
                TimeLeft = (int)(distance / 5),
                Status = MissionStatus.Proposal
            };
            await context.Missions.AddAsync(newMission);
            await context.SaveChangesAsync();
        }
        public double Distance(AgentModel agent, TargetModel target)
        {
            double distance = Math.Sqrt(Math.Pow(target.X - agent.X, 2) + Math.Pow(target.Y - agent.Y, 2));
            return distance;
        }
    }
}
