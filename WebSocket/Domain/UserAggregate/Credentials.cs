using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebSocket.Domain.Common;

namespace WebSocket.Domain.ValueObjects;

public class Credentials : ValueObject
{
    public string PasswordHash { get; init; } = string.Empty;
    
    public static bool PasswordVerify(string inputPassword, string hashedPassword) => BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
    public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public static Credentials Create(string password) => new Credentials(password);

    public Credentials() { }
    private Credentials(string password) 
    {
        PasswordHash =  HashPassword(password);    
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PasswordHash;
    }
};