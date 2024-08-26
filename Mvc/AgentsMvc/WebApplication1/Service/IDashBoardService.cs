using NuGet.Packaging.Signing;
using WebApplication1.Dto;

namespace WebApplication1.Service
{
	public interface IDashBoardService
	{
		Task<GeneralStatusDto> GeneralStatus();
		Task<List<AgentDto>> AgentsStatus();
		Task<List<TargetDto>> TargetsStatus();
		Task<MissionDto> MissionById(int missionId);
	}
}
