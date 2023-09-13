public static class EnvName
{
    public static string MapEnvToDatabase(string env)
    {
        return env switch
        {
            "Development" => "Dev",
            "Test" => "Test",
            _ => throw new NotImplementedException()
        };
    }
}