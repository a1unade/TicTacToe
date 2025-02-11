namespace TicTacToe.Infrastructure.Options;

public class RabbitOptions
{
    public string Username { get; set; } = default!;
    
    public string Password { get; set; } = default!;

    public string Hostname { get; set; } = default!;
}