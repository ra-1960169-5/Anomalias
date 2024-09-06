using System.Diagnostics.CodeAnalysis;

namespace Anomalias.Shared;

public class Result
{
 
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Errors = [error];
    }


    protected internal Result(bool isSuccess, Error[] errors)
    {

        IsSuccess = isSuccess;
        Errors = errors;
    }



    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

  
    public Error[] Errors { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, [error]);

    public static Result Failure(Error[] errors) => new(false, errors);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, [error]);

    public static Result<TValue> Failure<TValue>(Error[] errors) => new(default, false, errors);

    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);


    public static Result<T> Ensure<T>(T value, Func<T, bool> predicate, Error error)
    {
        return predicate(value) ?
              Success(value) :
              Failure<T>(error);
    }


    public static Result<T> Ensure<T>(T value, params (Func<T, bool> predicate, Error error)[] functions)
    {
        var results = new List<Result<T>>();

        foreach ((Func<T, bool> predicate, Error error) in functions)
        {
            results.Add(Ensure(value, predicate, error));
        }

        return Combine(results.ToArray());
    }

    public static Result<T> Combine<T>(params Result<T>[] results)
    {
        if (results.Any(r => r.IsFailure))
        {
            return Failure<T>(results.SelectMany(r => r.Errors).Where(e => e != Error.None).Distinct().ToArray());
        }
        return Success(results[0].Value);
    }


    public static Result<(T1, T2)> Combine<T1, T2>(Result<T1> result1, Result<T2> result2)
    {

        if (result1.IsFailure) return Failure<(T1, T2)>(result1.Errors);
        if (result2.IsFailure) return Failure<(T1, T2)>(result2.Errors);
        return Success((result1.Value, result2.Value));
    }

}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, [error])
    {
        _value = value;
    }

    protected internal Result(TValue? value, bool isSuccess, Error[] errors)
      : base(isSuccess, errors)
    {
        _value = value;
    }


    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}
