using NuGet.Versioning;

namespace WebApplication1.Dto
{
    public class GeneralStatusDto
    {
        public int AgentsQuantity { get; set; }
        public int ActiveAgents {  get; set; }
        public int TargetsQuantity { get; set; }
        public int EliminatedTargetsCount { get; set; }
        public int MissionsCount { get; set; }
        public int ActiveMissionsCount { get; set; }
        public float AgentsToTargetsRatio   { get; set; }
        public float InactiveAgentsToTargets { get; set; }

    }
}
