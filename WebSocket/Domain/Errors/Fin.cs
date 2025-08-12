namespace WebSocket.Domain.Errors;

public class Fin<T, R>
{
    private T? _value;
    private R? _error;
    public bool IsSuccess { get; }

    public T Value
    {
        get =>  IsSuccess ? _value! : throw new InvalidOperationException("Not successful");
        set => _value = value;
    }

    public R Error
    {
        get => !IsSuccess ? _error! : throw new InvalidOperationException("Not successful"); 
        set => _error = value;
    }

    private Fin(bool isSuccess, T? value, R? error) => (IsSuccess, _value, _error) = (isSuccess, value, error);

  
    public static implicit operator Fin<T, R>(T value) => new(true, value, default);
    public static implicit operator Fin<T, R>(R error) => new(false, default, error);
}

public static class Fin
{
    public static Fin<T, TR> Ok<T, TR>(T value) => value;
    public static Fin<T, TR> Fail<T, TR>(TR error) => error;
    public static Fin<TNew, TError> Map<T, TNew, TError>(this Fin<T, TError> result, Func<T, TNew> map)=>
        result.IsSuccess ? Ok<TNew, TError>(map(result.Value)) : Fail<TNew, TError>(result.Error);
    public static Fin<T, TNewError> MapError<T, TError, TNewError>(this Fin<T, TError> result, Func<TError, TNewError> map)=>
        result.IsSuccess ? Ok<T, TNewError>(result.Value) : Fail<T, TNewError>(map(result.Error));

    public static TResult Match<T, TError, TResult>(this Fin<T, TError> result, Func<T, TResult> mapValue,
        Func<TError, TResult> mapError) => 
            result.IsSuccess
            ? mapValue(result.Value)
            : mapError(result.Error);
    public static Fin<TNew, TError> Bind<T, TNew, TError>(this Fin<T, TError> result, Func<T, Fin<TNew, TError>> bind) =>
    result.IsSuccess ?  bind(result.Value) 
    : Fail<TNew, TError>(result.Error);
    
    
}
