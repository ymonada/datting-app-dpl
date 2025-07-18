using WebSocket.Domain.Common;

namespace WebSocket.Domain.UserAggregate;

public enum EmailStatus {
 Free = 10,
 Confirmed = 20,
 Blocked = 30
  
}

public class Email(string value, EmailStatus status) 
 : ValueObject
{
    public string Value { get; init; } = value;
    public EmailStatus Status { get; init; } =  status;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Status;
    }
}