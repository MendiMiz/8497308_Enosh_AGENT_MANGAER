using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Service
{
    public class TargetService(ApplicationDbContext context) : ITargetService
    {
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
    }
}
