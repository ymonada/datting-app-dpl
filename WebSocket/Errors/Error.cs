namespace WebSocket.Errors;

public enum ErrorCode
{
    NotFound,
    Validation,
    Unauthorized,
    Conflict,
    Internal
}
public class Error
{
    public ErrorCode Code { get; }
    public string Description { get; }

    public Error(ErrorCode code, string description)
    {
        Code = code;
        Description = description;
    }

    public override string ToString() => $"{Code}: {Description}";
    
    public static Error NotFound(string description = "Not found") =>
        new(ErrorCode.NotFound, description);

    public static Error Validation(string description) =>
        new(ErrorCode.Validation, description);

    public static Error Unauthorized(string description = "Unauthorized") =>
        new(ErrorCode.Unauthorized, description);

    public static Error Conflict(string description) =>
        new(ErrorCode.Conflict, description);

    public static Error Internal(string description = "Internal error") =>
        new(ErrorCode.Internal, description);
}
