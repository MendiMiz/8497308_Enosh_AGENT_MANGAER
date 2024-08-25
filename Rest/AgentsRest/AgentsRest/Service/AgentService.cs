﻿using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AgentsRest.Service
{
    public class AgentService(ApplicationDbContext context) : IAgentService
    {
        private readonly Dictionary<string, (int y, int x)> directionMove = new()
        {
            {"n", (1, 0) },
            {"ne", (1, 1) },
            {"e" , (0, 1) },
            {"se" , (-1, 1) },
            {"s" , (-1, 0) },
            {"sw" , (-1, -1) },
            {"w" , (0, -1) },
            {"nw" , (1, -1) }
        };
        public async Task MoveAgentToLocationAsync(AgentModel agent, int x, int y)
        {
            agent.X = x;
            agent.Y = y;
            await context.SaveChangesAsync();
        }
        public async Task AgentFirstLocation(LocationDto locationDto, int id)
        {
            AgentModel? agent = await context.Agents.FindAsync(id);
            if (agent == null) { throw new Exception("Not Found"); }
            await MoveAgentToLocationAsync(agent, locationDto.X, locationDto.Y);
        }

        public async Task<int> CreateAgentReturnIdAsync(AgentDto agentDto)
        {
            if (agentDto == null) { throw new ArgumentNullException(nameof(agentDto)); }
            AgentModel agentModel = new()
            {
                Nickname = agentDto.Nickname,
                Image = agentDto.PhotoUrl,
                Status = StatusAgent.Inactive
            };
            await context.AddAsync(agentModel);
            await context.SaveChangesAsync();
            return agentModel.Id;
        }

        public async Task<List<AgentModel>> GetAgentsAsync()
        {
            List<AgentModel> agents = await context.Agents.ToListAsync();
            return agents;
        }

        public async Task MoveAgentById(int agentId, DirectionDto direction)
        {
            AgentModel? agent = await context.Agents.FindAsync(agentId);
            if (agent == null) { throw new Exception("Not Found"); }
            if (agent.Status == StatusAgent.Active) { throw new Exception("In mission"); }
            int x = agent.X;
            int y = agent.Y;
            (int y, int x) move = directionMove[direction.Direction];
            x += move.x;
            y += move.y;
            if(x < 0 || x > 1000 || y < 0 || y > 1000) { throw new Exception("Out of range"); }
            await MoveAgentToLocationAsync(agent, x, y);
        }

        public double Distance(AgentModel agent, TargetModel target)
        {
            double distance = Math.Sqrt(Math.Pow(target.X - agent.X, 2) + Math.Pow(target.Y - agent.Y, 2));
            return distance;
        }

    

        //public async Task<List<MissionModel>> ActualMissions()
        //{
        //    //Get inactive agents
        //    List<AgentModel> agents = await context.Agents.Where(a => a.Status == StatusAgent.Inactive).ToListAsync();
        //    List<TargetModel> targets = await context.Targets.Where(t => true == !context.Missions.Any(m => m.TargetId == t.Id)

        //}

        //public async Task<bool> MoveIfInRange(DirectionDto direction, int CurrentX, int CurrentY)
        //{
        //    //Get the moves needed to go to the direction given
        //    (int y, int x) move = directionMove[direction.Direction];
        //    int newX = CurrentX + move.x;
        //    int newY = CurrentY + move.y;
        //    if (newX < 0 || newX > 1000 || newY < 0 || newY > 1000) { return false; }

        //}
    }
}
