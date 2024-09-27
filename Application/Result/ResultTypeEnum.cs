namespace Application.Result;

public enum ResultType
{
    Ok = 200,
    Unexpected = 502, // Sólo para 500 ajenos
    NotFound = 404,
    Unauthorized = 401,
    Invalid = 500
}