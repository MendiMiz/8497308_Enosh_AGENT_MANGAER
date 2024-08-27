using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using static AgentsRest.Utils.movesAndCalculations;

namespace AgentsRest.Service
{
    public class AgentService(ApplicationDbContext context) : IAgentService
    {
        public async Task MoveAgentToLocationAsync(AgentModel agent, int x, int y)
        {
            agent.X = x;
            agent.Y = y;
            await context.SaveChangesAsync();
        }
        //give to a new created agent a position
        public async Task AgentFirstLocation(LocationDto locationDto, int id)
        {
            AgentModel? agent = await context.Agents.FindAsync(id);
            if (agent == null) { throw new Exception("Not Found"); }
            await MoveAgentToLocationAsync(agent, locationDto.X, locationDto.Y);
        }
        
        public async Task<int> CreateAgentReturnIdAsync(AgentDto agentDto)
        {
            if (agentDto == null) { throw new ArgumentNullException(nameof(agentDto)); }
            AgentModel agentModel = new()
            {
                Nickname = agentDto.Nickname,
                Image = agentDto.PhotoUrl,
                Status = StatusAgent.Inactive
            };
            await context.AddAsync(agentModel);
            await context.SaveChangesAsync();
            return agentModel.Id;
        }

        public async Task<List<AgentModel>> GetAgentsAsync()
        {
            List<AgentModel> agents = await context.Agents.ToListAsync();
            return agents;
        }

        public async Task MoveAgentById(int agentId, DirectionDto direction)
        {
            AgentModel? agent = await context.Agents.FindAsync(agentId);
            if (agent == null) { throw new Exception("Not Found"); }
            if (agent.Status == StatusAgent.Active) { throw new Exception("In mission"); }
            //add the needed + to the new x and y
            int x = agent.X;
            int y = agent.Y;
            (int y, int x) move = directionMove[direction.Direction];
            x += move.x;
            y += move.y;
            //throw an error if movement is out of map range.
            if(x < 0 || x > 1000 || y < 0 || y > 1000) { throw new Exception("Out of range"); }
            //using the new x and y to update the agent new position
            await MoveAgentToLocationAsync(agent, x, y);
        }

        public double Distance(AgentModel agent, TargetModel target)
        {
            double distance = Math.Sqrt(Math.Pow(target.X - agent.X, 2) + Math.Pow(target.Y - agent.Y, 2));
            return distance;
        }
    }
}
