using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication1.Dto;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class MissionsSelectController(IMissionsSelectService missionsSelectService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<MissionDto> missionsToView = await missionsSelectService.GetOrderedMissionProposal();
            return View(missionsToView);
        }

        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await missionsSelectService.ActivateMission(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
