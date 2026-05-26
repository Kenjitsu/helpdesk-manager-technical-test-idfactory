using System.Net;

namespace HelpDeskManager.Core.DTOs.Results;

public class Result<T>
{
    public bool IsSuccess => Error == null;

    public int StatusCode { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public Error? Error { get; init; }

    public static Result<T> Failure(Error error, HttpStatusCode? statusCode = null, string? message = null) 
        => new() { Error = error, Data = default, StatusCode = (int)(statusCode ?? HttpStatusCode.BadRequest), Message = message ?? "Failure" };
    public static Result<T> Success(T data, HttpStatusCode? statusCode = null, string? message = null) 
        => new() { StatusCode = (int)(statusCode ?? HttpStatusCode.OK), Message = message ?? "Success", Data = data };

    public TResult Match<TResult>(Func<Result<T>, TResult> onSuccess, Func<Result<T>, TResult> onFailure)
    {
        return IsSuccess
            ? onSuccess(this)
            : onFailure(this);
    }
}
