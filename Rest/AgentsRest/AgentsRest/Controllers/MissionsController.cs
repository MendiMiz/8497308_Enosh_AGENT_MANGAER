using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(IMissionService _missionService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAgentsList()
        {
            try
            {
                List<MissionModel> missions = await _missionService.GetMissionsAsync();
                return Ok(missions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("update")]
        public async Task<ActionResult> UpdateMissions()
        {
            try
            {
                await _missionService.UpdateMissions();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("activate/{missionId}")]
        public async Task<ActionResult> ActivateMission(int missionId)
        {
            try
            {
                await _missionService.ActivateMissionById(missionId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("proposalMissions")]
        public async Task<ActionResult> GetProposalMissions()
        {
            try
            {
                List<MissionModel> proposalMissions = await _missionService.GetProposalMissions();
                return Ok(proposalMissions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
