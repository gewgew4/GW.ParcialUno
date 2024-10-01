namespace Application.Result;

public enum ResultType
{
    Ok = 200,
    Unexpected = 502, // External service error
    NotFound = 404,
    Unauthorized = 401,
    Invalid = 500
}