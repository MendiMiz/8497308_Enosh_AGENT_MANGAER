namespace WebApplication1.Dto
{
    public enum MissionStatus
    {
        Proposal,
        InMission,
        Completed
    }
    public class MissionDto
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public AgentDto Agent { get; set; }
        public int TargetId { get; set; }
        public TargetDto Target { get; set; }
        public int TimeLeft { get; set; }
        public int Distance { get; set; }
        public DateTime TimeOfKill { get; set; }
        public MissionStatus Status { get; set; }
    }
}
