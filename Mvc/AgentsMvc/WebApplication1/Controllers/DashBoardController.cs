using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class DashBoardController(IDashBoardService dashBoardService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            GeneralStatusDto generalStatus = await dashBoardService.GeneralStatus();
            return View(generalStatus);
        }
		public async Task<IActionResult> TargetsStatus()
		{
			List<TargetDto> targets = await dashBoardService.TargetsStatus();
			return View(targets);
		}
		public async Task<IActionResult> AgentsStatus()
        {
            List<AgentDto> agents = await dashBoardService.AgentsStatus();
            return View(agents);
        }

        public async Task<IActionResult> MissionDetails(int missionId)
        {
            MissionDto mission = await dashBoardService.MissionById(missionId);
            return View(mission);
        }

    }
}
