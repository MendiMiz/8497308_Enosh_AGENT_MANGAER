namespace AgentsRest.Service
{
    public interface IJwtService
    {
        string CreateToken(string name);
    }
}
