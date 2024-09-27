namespace Application.Result;

/// <summary>
/// Result model to contain data, status code, string message and succeess boolean 
/// </summary>
public class Result<T> where T : class
{
    public bool Success { set; get; }
    public List<string>? Errors { set; get; }
    public T? Data { set; get; }
    public ResultType ResultType { set; get; }

    public static Result<T> Ok(T? data)
    {
        return new Result<T>()
        {
            Success = true,
            Errors = null,
            Data = data,
            ResultType = ResultType.Ok
        };
    }

    public static Result<T> Fail(ResultType resultType, List<string>? errors = null)
    {
        return new Result<T>()
        {
            Success = false,
            Errors = errors,
            Data = default(T),
            ResultType = resultType
        };
    }
}