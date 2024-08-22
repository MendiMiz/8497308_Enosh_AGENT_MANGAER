using AgentsRest.Dto;
using AgentsRest.Models;
using System.Security.Cryptography.Xml;

namespace AgentsRest.Service
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAgentsAsync();
        Task<int> CreateAgentReturnIdAsync(AgentDto agentDto);
        Task AgentFirstLocation(LocationDto locationDto, int id);

    }
}
