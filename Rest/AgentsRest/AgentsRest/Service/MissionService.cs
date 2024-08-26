using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using static AgentsRest.Utils.movesAndCalculations;

namespace AgentsRest.Service
{
    public class MissionService(ApplicationDbContext context) : IMissionService
    {
        public async Task<List<MissionModel>> ActualMissionProposalWhenAgentMove(int agentId)
        {
            AgentModel? agent = await context.Agents.FindAsync(agentId);
            if (agent == null) { throw new Exception($"failed to find a user with {agentId} id"); }
            //id of targets that are not killed or inprogress
            List<TargetModel> targetsToGiveProposal = await context.Targets
                .Where(t => t.Status == StatusTarget.Alive)
                .ToListAsync();
            //list of targets id already in proposal to this agent
            List<int> targetsAlreadyInProposal = await context.Missions
                .Where(m => m.AgentId == agentId)
                .Select(m => m.TargetId)
                .ToListAsync();

            foreach (TargetModel target in targetsToGiveProposal)
            {
                //check to not make double proposal
                if (!targetsAlreadyInProposal.Contains(target.Id))
                {
                    //check the distance from the target
                    double AgentToTargetDistance = Distance(agent, target);
                    if (AgentToTargetDistance <= 200)
                    {
                        await CreateMission(agent.Id, target.Id, AgentToTargetDistance);
                    }
                }
            }
            await context.SaveChangesAsync();
            //check if agents with proposal go out of the range  and remove from proposal
            await CheckIfAgentGoOutOfRangeAndRemove();

            
            return await context.Missions.Where(m => m.Status == MissionStatus.Proposal).ToListAsync();
        }

        private async Task CheckIfAgentGoOutOfRangeAndRemove()
        {
            List<MissionModel> missions = await context.Missions
                .Include(m => m.Agent)
                .Include(m => m.Target)
                .Where(m => m.Status == MissionStatus.Proposal)
                .ToListAsync();
            foreach (MissionModel mission in missions)
            {
                double agentToTargetDistance = Distance(mission.Agent, mission.Target);
                if (agentToTargetDistance > 200) context.Missions.Remove(mission);
            }
            await context.SaveChangesAsync();
        }
        public async Task<List<MissionModel>> ActualMissionProposalWhenTargetMove(int targetId)
        {
            TargetModel? target = await context.Targets.FindAsync(targetId);
            if (target == null) { throw new Exception($"failed to find a user with {targetId} id"); }

            if (target.Status == StatusTarget.Alive)
            {
                List<AgentModel> inactiveAgents = await context.Agents
                    .Where(agent => agent.Status == StatusAgent.Inactive).ToListAsync();

                //inactive agents with proposal to this target
                List<AgentModel> inactiveAgentsWithPropasal = await context.Missions
                    .Include(m => m.Agent)
                    .Where(m => m.TargetId == targetId)
                    .Select(m => m.Agent)
                    .ToListAsync();

                //take them out of the list of inactive agents
                foreach (AgentModel agent in inactiveAgents)
                {
                    if (inactiveAgentsWithPropasal.Contains(agent))
                    {
                        inactiveAgents.Remove(agent);
                    }
                }
                //check if the agents are in range to give proposal
                foreach (AgentModel agent in inactiveAgents)
                {
                    double AgentToTargetDistance = Distance(agent, target);
                    if (AgentToTargetDistance <= 200)
                    {
                        await CreateMission(agent.Id, target.Id, AgentToTargetDistance);
                    }
                }
            }
            await context.SaveChangesAsync();
            await CheckIfAgentGoOutOfRangeAndRemove();
            
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
        public async Task UpdateMissions()
        {
            List<MissionModel> missionsInProccess = await context.Missions
                .Where(m => m.Status == MissionStatus.InMission)
                .Include(m => m.Agent).Include(m => m.Target)
                .ToListAsync();
            foreach (MissionModel mission in missionsInProccess)
            {
                await MoveAgentTowardsTargetOrKill(mission);
            }
        }
        private string BestMoveTowardstarget(AgentModel agent, TargetModel target)
        {
            int differenceX = agent.X - target.X;
            int differenceY = agent.Y - target.Y;

            if (differenceY > 0)
            {
                if (differenceX > 0) return "sw";
                else if (differenceX < 0) return "se";
                else return "s";
            }
            if (differenceY < 0)
            {
                if (differenceX > 0) return "nw";
                else if (differenceX < 0) return "ne";
                else return "n";
            }
            else
            {
                if (differenceX > 0) return "w";
                else return "e";
            }
        }
        public async Task MoveAgentToLocationAsync(AgentModel agent, int x, int y)
        {
            agent.X = x;
            agent.Y = y;
            await context.SaveChangesAsync();
        }
        public async Task MoveAgentById(int agentId, string direction)
        {
            AgentModel? agent = await context.Agents.FindAsync(agentId);
            if (agent == null) { throw new Exception("Not Found"); }
            int x = agent.X;
            int y = agent.Y;
            (int y, int x) move = directionMove[direction];
            x += move.x;
            y += move.y;
            if (x < 0 || x > 1000 || y < 0 || y > 1000) { throw new Exception("Out of range"); }
            await MoveAgentToLocationAsync(agent, x, y);
        }
        private async Task MoveAgentTowardsTargetOrKill(MissionModel mission)
        {
            string bestMoveDirection = BestMoveTowardstarget(mission.Agent, mission.Target);
            await MoveAgentById(mission.Agent.Id, bestMoveDirection);
            if (mission.Agent.X == mission.Target.X && mission.Agent.Y == mission.Target.Y)
            {
                mission.Target.Status = StatusTarget.Killed;
                mission.Agent.Status = StatusAgent.Inactive;
                mission.Status = MissionStatus.Completed;
                mission.TimeOfKill = DateTime.Now;
                await context.SaveChangesAsync();
            }
            else
            {
                mission.TimeLeft = (int)Distance(mission.Agent, mission.Target) / 5;
                await context.SaveChangesAsync();
            }

        }
        public async Task ActivateMissionById(int missionId)
        {
            MissionModel? mission = await context.Missions
                .Include(m => m.Agent).Include(m => m.Target)
                .FirstOrDefaultAsync(m => m.Id == missionId);
            if (mission == null) { throw new Exception($"there is no a mission with ({missionId}) as id"); }

            mission.Status = MissionStatus.InMission;
            mission.Agent.Status = StatusAgent.Active;
            mission.Target.Status = StatusTarget.InProgress;

            List<MissionModel> missionsProposalToDelete = await context.Missions
               .Where(m => m.TargetId == mission.TargetId || m.AgentId == mission.AgentId)
               .Where(m => m.Id != missionId)
               .ToListAsync();

            context.RemoveRange(missionsProposalToDelete);
            await context.SaveChangesAsync();

        }
        public async Task<List<MissionModel>> GetProposalMissions()
        {
            return await context.Missions
                .Where(m => m.Status == MissionStatus.Proposal)
                .Include(m => m.Agent)
                .Include(m => m.Target)
                .ToListAsync();
        }
        public async Task<List<MissionModel>> GetMissionsAsync()
        {
            return await context.Missions
                .Include(m => m.Agent)
                .Include(m => m.Target)
                .ToListAsync();
        }
    }
}
