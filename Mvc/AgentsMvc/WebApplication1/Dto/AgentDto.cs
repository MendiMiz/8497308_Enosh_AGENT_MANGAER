namespace WebApplication1.Dto
{
    public enum StatusAgent
    {
        Active,
        Inactive
    }
    public class AgentDto
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public StatusAgent Status { get; set; }
        public int Kills { get; set; }
        public int TimeLeftToMission { get; set; }
        public int MissionId {  get; set; }
    }
}
