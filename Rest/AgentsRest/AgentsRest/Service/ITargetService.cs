using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetTargetsAsync();
        Task<int> CreateTargetReturnIdAsync(TargetDto targetDto);
        Task TargetFirstLocation(LocationDto locationDto, int id);
        Task MoveTargetById(int targetId, DirectionDto direction);

    }
}
