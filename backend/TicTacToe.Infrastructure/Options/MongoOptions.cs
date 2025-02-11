namespace TicTacToe.Infrastructure.Options;

public class MongoOptions
{
    public string ConnectionString { get; set; } = default!;

    public string DatabaseName { get; set; } = default!;

    public string CollectionName { get; set; } = default!;
}