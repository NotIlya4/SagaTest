namespace ExecutionStrategyExtended.Extensions;

public record IdempotencyTokenTableOptions
{
    public int MaxLength { get; set; } = 255;
    public string TableName { get; set; } = "IdempotencyTokens";
    public string PrimaryKeyConstraintName { get; set; } = "PK_IdempotencyTokens";
}