namespace AgentsRest.Models
{
    public enum StatusAgent
    {
        Active,
        Inactive
    }
    public class AgentModel
    {
        public int Id {  get; set; }
        public string Nickname { get; set; }    
        public string Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public StatusAgent Status { get; set; }
    }
}
