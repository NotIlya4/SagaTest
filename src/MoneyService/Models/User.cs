using MoneyService.Extensions;

namespace MoneyService.Models;

public record User
{
    public int Id { get; private set; }
    public decimal Money { get; set; }
    public bool IsBlocked { get; private set; }

    protected User() { }
    
    public User(int id, decimal money, bool isBlocked)
    {
        Id = id;
        Money = money;
        IsBlocked = isBlocked;
    }

    public void AddMoney(decimal deltaMoney)
    {
        ValidateIsBlocked();
        
        var finalMoney = Money + deltaMoney;

        if (finalMoney < 0)
        {
            ExceptionThrower.ThrowAttemptTakeMoneyMoreThanUserHas();
        }

        Money = finalMoney;
    }

    public void ValidateIsBlocked()
    {
        if (IsBlocked)
        {
            ExceptionThrower.ThrowUserIsBlocked();
        }
    }
}