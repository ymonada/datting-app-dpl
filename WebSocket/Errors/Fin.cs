using System.Collections;
using System.Text.RegularExpressions;

namespace WebSocket.Errors;

public class Fin<T, R>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public R? Error { get; }

    private Fin(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = default;
    }

    private Fin(R error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<R, TResult> onFailure)
    {
        return IsSuccess
            ? onSuccess(Value!)
            : onFailure(Error!);
    }
    public Fin<T, R> ForEachError(Action<R> action)
    {
        if (!IsSuccess && Error is IEnumerable)
            action(Error);
        return this;
    }

    public static implicit operator Fin<T, R>(T value) => new(value);
    public static implicit operator Fin<T, R>(R error) => new(error);
}

public static class Fin
{
    public static Fin<T, R> Ok<T, R>(T value) => value;
    public static Fin<T, R> Fail<T, R>(R error) => error;
}
