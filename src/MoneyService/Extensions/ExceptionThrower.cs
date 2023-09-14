namespace MoneyService;

public static class ExceptionThrower
{
    public static void ThrowAttemptTakeMoneyMoreThanUserHas()
    {
        throw new InvalidOperationException("You can't take money more than you have");
    }

    public static void ThrowUserIsBlocked()
    {
        throw new InvalidOperationException("User is blocked");
    }
}