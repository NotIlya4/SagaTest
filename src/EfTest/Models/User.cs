namespace EfTest.Models;

public record User
{
    public string Id { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    protected User() { }
    
    public User(string id, string name)
    {
        Id = id;
        Name = name;
    }
}