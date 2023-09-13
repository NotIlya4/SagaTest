namespace EfTest.Models;

public record User
{
    public int Id { get; private set; }
    public decimal Money { get; private set; }
    public bool IsBlocked { get; private set; }

    protected User() { }
    
    public User(int id, decimal money, bool isBlocked)
    {
        Id = id;
        Money = money;
        IsBlocked = isBlocked;
    }
}