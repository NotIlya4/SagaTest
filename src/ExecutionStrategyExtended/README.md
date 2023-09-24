# ExecutionStrategyExtended
Это небольшой велосипед поверх `IExecutionStrategy`, который добавляет всякие плюшки, а также автоматически обрабатывает ключи идемпотентности в отдельной таблице для идемпотентных транзакций.

Нафига я пересоздаю контекст на каждый ретрай. Вот есть пример в котором очень легко можно словить баг, если просто использовать голый `IExecutionStrategy.ExecuteAsync`:
```csharp
strategy.ExecuteAsync(
    async (context) =>
    {
        var user = new User(
            id: 0,
            name: "biba");

        context.Add(user);

        // Тут может упасть TimoutException и IExecutionStrategy заретраит
        var products = await context.Products.ToListAsync();

        await context.SaveChangesAsync();
    });
```
Здесь при таймаут эксепшене при получении продуктов IExecutionStrategy заретраит и на второй попытке будет добавлен второй юзер в контекст, при SaveChanges 2 юзера будут сохранены в бд.
