namespace AgentsRest.Dto
{
    public enum Direction
    {
        NW,
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W
    }

    public class DirectionDto
    {
        public Direction Direction { get; set; }    
    }
}
