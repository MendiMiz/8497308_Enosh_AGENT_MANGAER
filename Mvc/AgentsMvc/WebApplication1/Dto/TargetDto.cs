namespace WebApplication1.Dto
{
    public enum StatusTarget
    {
        Alive,
        InProgress,
        Killed
    }
    public class TargetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public StatusTarget Status { get; set; }

    }
}
