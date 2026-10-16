namespace AccountStatements.Infrastructure.Email;

public class EmailSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 25;
    public bool EnableTls { get; set; } = true;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string FromName { get; set; } = "Statements";
    public string FromAddress { get; set; } = "statements@example.com";
}
