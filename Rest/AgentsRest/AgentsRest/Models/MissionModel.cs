namespace AgentsRest.Models
{
    public enum MissionStatus
    {
        Proposal,
        InMission,
        Completed
    }
    public class MissionModel
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public AgentModel Agent { get; set; }
        public int TargetId { get; set; }
        public TargetModel Target { get; set; }
        public int TimeLeft { get; set; }
        public DateTime TimeOfKill { get; set; }
        public MissionStatus Status { get; set; }
    }
}
