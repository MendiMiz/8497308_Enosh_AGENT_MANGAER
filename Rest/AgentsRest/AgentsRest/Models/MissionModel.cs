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
        public DateTime TimeLeft { get; set; }
        public DateTime TimeToDo { get; set; }
        public MissionStatus MissionStatus { get; set; }
    }
}
