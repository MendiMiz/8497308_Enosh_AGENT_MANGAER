using System.Collections.Generic;
using System.Text.Json;
using WebApplication1.Dto;

namespace WebApplication1.Service
{
    public class DashBoardService(IHttpClientFactory clientFactory) : IDashBoardService
    {

        public async Task<GeneralStatusDto> GeneralStatus()
        {
            List<MissionDto> missions = await AllMissions();
            List<AgentDto> agents = await AllAgents();
            List<TargetDto> targets = await AllTargets();
            GeneralStatusDto generalStatus = new()
            {
                AgentsQuantity = agents.Count(),
                ActiveAgents = agents.Where(a => a.Status == StatusAgent.Active).Count(),
                TargetsQuantity = targets.Count(),
                EliminatedTargetsCount = targets.Where(t => t.Status == StatusTarget.Killed).Count(),
                MissionsCount = missions.Count(),
                ActiveMissionsCount = missions.Where(m => m.Status == MissionStatus.InMission).Count(),
                AgentsToTargetsRatio = agents.Count() / targets.Count(),
                InactiveAgentsToTargets = agents.Where(a => a.Status == StatusAgent.Inactive).Count() / targets.Count(),
            };
            return generalStatus;
        }
        private async Task<List<MissionDto>> AllMissions()
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:7185/Missions");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<MissionDto>? missions = JsonSerializer.Deserialize<List<MissionDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
                if (missions != null) return missions;
            }
            return [];
        }
        private async Task<List<AgentDto>> AllAgents()
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:7185/Agents");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<AgentDto>? agents = JsonSerializer.Deserialize<List<AgentDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
                if (agents != null) return agents;
            }
            return [];
        }
        private async Task<List<TargetDto>> AllTargets()
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:7185/Targets");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<TargetDto>? targets = JsonSerializer.Deserialize<List<TargetDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
                if (targets != null) return targets;
            }
            return [];
        }
        public async Task<List<AgentDto>> AgentsStatus()
        {
            List<MissionDto> missions = await AllMissions();
            List<AgentDto> agents = await AllAgents();
            foreach (AgentDto agent in agents)
            {
                agent.Kills = missions
                    .Where(m => m.Status == MissionStatus.Completed)
                    .Where(m => m.AgentId == agent.Id)
                    .Count();
                if (agent.Status == StatusAgent.Active)
                {
                    MissionDto mission = missions.Where(m => m.AgentId == agent.Id).First();
                    agent.TimeLeftToMission = mission.TimeLeft ;
                    agent.MissionId =mission.Id;
                }
            }
            return agents;
        }
        public async Task<MissionDto> MissionById(int missionId)
        {
            List<MissionDto> missions = await AllMissions();
            MissionDto mission = missions.First(m => m.Id == missionId);
            mission.Distance = Distance(mission);
            return mission;
        }

        private int Distance(MissionDto mission)
        {
            return mission.TimeLeft * 5;
        }

		public async Task<List<TargetDto>> TargetsStatus()
		{
			return await AllTargets();
		}
	}
}
