using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Service
{
    public class AgentService(ApplicationDbContext context) : IAgentService
    {
        public async Task AgentFirstLocation(LocationDto locationDto, int id)
        {
            AgentModel? agent = await context.Agents.FindAsync(id);
            if (agent == null) { throw new Exception("Not Found"); }
            agent.X = locationDto.X;
            agent.Y = locationDto.Y;
            await context.SaveChangesAsync();
        }

        public async Task<int> CreateAgentReturnIdAsync(AgentDto agentDto)
        {
            if (agentDto == null) { throw new ArgumentNullException(nameof(agentDto)); }
            AgentModel agentModel = new()
            {
                Nickname = agentDto.Nickname,
                Image = agentDto.Photo_Url
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
    }
}
