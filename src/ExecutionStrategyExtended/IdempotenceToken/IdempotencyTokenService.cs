using ExecutionStrategyExtended.Models;

namespace ExecutionStrategyExtended;

internal class IdempotencyTokenService
{
    private readonly IdempotencyTokenRepository _repository;
    private readonly IdempotencyTokenManager _tokenManager;

    public IdempotencyTokenService(IdempotencyTokenRepository repository, IdempotencyTokenManager tokenManager)
    {
        _repository = repository;
        _tokenManager = tokenManager;
    }
    
    public async Task<TResponse> HandleAction<TResponse>(Func<Task<TResponse>> action, 
        IdempotencyToken idempotencyToken)
    {
        var addResult = await _repository.AddAndSaveToken(idempotencyToken);

        if (addResult.IsAlreadyExists)
        {
            var freshTokenFromDb = await _repository.GetFreshToken(idempotencyToken);
            return _tokenManager.GetResponse<TResponse>(freshTokenFromDb);
        }

        var response = await action();
        _tokenManager.SetResponse(idempotencyToken, response);

        await _repository.UpdateAndSaveToken(idempotencyToken);

        return response;
    }
}