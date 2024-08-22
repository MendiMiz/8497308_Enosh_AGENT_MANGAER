using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json;

namespace AgentsRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController(IAgentService agentService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult> GetAgentsList()
        {
            try
            {
                List<AgentModel> agents = await agentService.GetAgentsAsync();
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
                int newAgentId = await agentService.CreateAgentReturnIdAsync(agentDto);
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
                await agentService.AgentFirstLocation(locationDto, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
