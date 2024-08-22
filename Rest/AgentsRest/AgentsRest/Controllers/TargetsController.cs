using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController(ITargetService targetService ) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetTargetsList()
        {
            try
            {
                List<TargetModel> targets = await targetService.GetTargetsAsync();
                return Ok(targets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewTarget([FromBody] TargetDto targetDto)
        {
            try
            {
                int newTargetId = await targetService.CreateTargetReturnIdAsync(targetDto);
                IdDto targetIdToSend = new() { Id = newTargetId };
                return Ok(targetIdToSend);
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
                await targetService.TargetFirstLocation(locationDto, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}
