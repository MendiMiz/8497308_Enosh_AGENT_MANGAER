using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController(IAgentService _agentService, IMissionService _missionService /*IServiceProvider serviceProvider*/) : Controller
    {
        //private IAgentService _agentService = serviceProvider.GetRequiredService<IAgentService>();
        //private IMissionService _missionService = serviceProvider.GetRequiredService<IMissionService>();
        [HttpGet]
        public async Task<ActionResult> GetAgentsList()
        {
            try
            {
                List<AgentModel> agents = await _agentService.GetAgentsAsync();
                return Ok(agents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewAgent([FromBody] AgentDto agentDto)
        {
            try
            {
                int newAgentId = await _agentService.CreateAgentReturnIdAsync(agentDto);
                IdDto agentIdToSend = new() { Id = newAgentId };
                return Ok(agentIdToSend);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/pin")]
        public async Task<ActionResult> AddAgentFirstLocation([FromBody] LocationDto locationDto, int id)
        {
            try
            {
                await _agentService.AgentFirstLocation(locationDto, id);
                await _missionService.ActualMissionProposalWhenAgentMove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/move")]
        public async Task<ActionResult> UpdateAgentLocation([FromBody] DirectionDto direction, int id)
        {
            try
            {
                await _agentService.MoveAgentById(id, direction);
                await _missionService.ActualMissionProposalWhenAgentMove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
