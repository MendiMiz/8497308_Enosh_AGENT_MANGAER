using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using static AgentsRest.Utils.movesAndCalculations;

namespace AgentsRest.Service
{
    public class TargetService(ApplicationDbContext context) : ITargetService
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
        public async Task<List<TargetModel>> GetTargetsAsync()
        {
            List<TargetModel> targets = await context.Targets.ToListAsync();
            return targets;
        }

        public async Task<int> CreateTargetReturnIdAsync(TargetDto targetDto)
        {
            if (targetDto == null) { throw new ArgumentNullException(nameof(targetDto)); }
            TargetModel targetModel = new()
            {
                Name = targetDto.Name,
                Role = targetDto.Position,
                Image = targetDto.PhotoUrl
            };
            await context.AddAsync(targetModel);
            await context.SaveChangesAsync();
            return targetModel.Id;
        }

        public async Task TargetFirstLocation(LocationDto locationDto, int id)
        {
            TargetModel? target = await context.Targets.FindAsync(id);
            if (target == null) { throw new Exception("Not Found"); }
            target.X = locationDto.X;
            target.Y = locationDto.Y;
            await context.SaveChangesAsync();
        }
        public async Task MoveAgentById(int targetId, DirectionDto direction)
        {
            TargetModel? target = await context.Targets.FindAsync(targetId);
            if (target == null) { throw new Exception("Not Found"); }
            if (target.Status == StatusTarget.Killed) { throw new Exception("Killed"); }
            int x = target.X;
            int y = target.Y;
            (int y, int x) move = directionMove[direction.Direction];
            x += move.x;
            y += move.y;
            if (x < 0 || x > 1000 || y < 0 || y > 1000) { throw new Exception("Out of range"); }
            await MoveTargetToLocationAsync(target, x, y);
        }

        public async Task MoveTargetToLocationAsync(TargetModel target, int x, int y)
        {
            target.X = x;
            target.Y = y;
            await context.SaveChangesAsync();
        }
    }
}
