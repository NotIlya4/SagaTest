using Microsoft.EntityFrameworkCore.Storage;

namespace ExecutionStrategyExtended.TransactionRetrier;

internal interface ITransactionRetrier
{
    public Task OnTransactionFail(IDbContextTransaction? transaction)
    {
        
    }
}