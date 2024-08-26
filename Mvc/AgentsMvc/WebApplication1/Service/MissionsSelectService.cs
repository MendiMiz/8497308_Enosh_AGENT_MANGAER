using System.Text.Json;
using WebApplication1.Dto;

namespace WebApplication1.Service
{
    public class MissionsSelectService(IHttpClientFactory clientFactory) : IMissionsSelectService
    {
        public async Task<List<MissionDto>> GetOrderedMissionProposal()
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:7185/Missions/proposalMissions");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<MissionDto>? proposalMissions = JsonSerializer.Deserialize<List<MissionDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );

                if (proposalMissions != null)
                {
                    foreach (MissionDto item in proposalMissions)
                    {
                        item.Distance = item.TimeLeft * 5;
                    }
                    List<MissionDto> orderedList = proposalMissions.OrderBy(m => m.Distance).ToList();
                    return orderedList;
                }       
            }
            return [];
        }

        public async Task ActivateMission(int missionId)
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.PostAsync($"https://localhost:7185/Missions/activate/{missionId}", null);
            if (!response.IsSuccessStatusCode) { throw new Exception("failed to activate mission"); }
        }
    }
}
