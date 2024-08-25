namespace AgentsRest.Utils
{
    public class movesAndCalculations
    {
        private readonly Dictionary<string, (int y, int x)> directionMove = new()
        {
            {"n", (1, 0) },
            {"ne", (1, 1) },
            {"e" , (0, 1) },
            {"se" , (-1, 1) },
            {"s" , (-1, 0) },
            {"sw" , (-1, -1) },
            {"w" , (0, -1) },
            {"nw" , (1, -1) }
        };
    }
}
