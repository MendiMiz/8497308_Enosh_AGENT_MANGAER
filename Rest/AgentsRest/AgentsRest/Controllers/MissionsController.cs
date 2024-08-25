using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(IMissionService _missionService) : ControllerBase
    {
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

    }
}
