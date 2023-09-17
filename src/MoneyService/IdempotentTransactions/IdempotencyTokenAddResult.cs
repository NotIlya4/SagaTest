using OneOf;
using OneOf.Types;

namespace MoneyService.IdempotentTransactions;

[GenerateOneOf]
public partial class IdempotencyTokenAddResult : OneOfBase<Success, AlreadyExists, UnknownException>
{
    public void ThrowIfUnknownException()
    {
        if (Value is UnknownException unknownException)
        {
            throw unknownException.Exception;
        }
    }
}