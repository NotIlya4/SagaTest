namespace ExecutionStrategyExtended.Configuration;

public record IdempotencyTokenTableConfiguration
{
    public int MaxLength { get; set; } = 255;
    public string TableName { get; set; } = "IdempotencyTokens";
    public string PrimaryKeyConstraintName { get; set; } = "PK_IdempotencyTokens";

    public IdempotencyTokenTableConfiguration WithMaxLength(int maxLength)
    {
        MaxLength = maxLength;
        return this;
    }
    
    public IdempotencyTokenTableConfiguration WithTableName(string tableName)
    {
        TableName = tableName;
        return this;
    }
    
    public IdempotencyTokenTableConfiguration WithPrimaryKeyConstraintName(string primaryKeyConstraintName)
    {
        PrimaryKeyConstraintName = primaryKeyConstraintName;
        return this;
    }
}