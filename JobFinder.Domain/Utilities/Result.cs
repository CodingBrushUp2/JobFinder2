namespace JobFinder.Domain.Utilities;

public class Result<T>
{
    public T Value { get; }
    public bool IsSuccess { get; }
    public string Error { get; }
    public int StatusCode { get; }

    protected Result(T value, bool isSuccess, string error, int statusCode)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T value, int statusCode = 200)
    {
        return new Result<T>(value, true, string.Empty, statusCode);
    }

    public static Result<T> Failure(string error, int statusCode)
    {
        return new Result<T>(default, false, error, statusCode);
    }
}

public class Result : Result<object>
{
    protected Result(object value, bool isSuccess, string error, int statusCode)
        : base(value, isSuccess, error, statusCode)
    {
    }

    public static new Result Success(int statusCode = 204)
    {
        return new Result(null, true, string.Empty, statusCode);
    }

    public static new Result Failure(string error, int statusCode)
    {
        return new Result(null, false, error, statusCode);
    }
}
