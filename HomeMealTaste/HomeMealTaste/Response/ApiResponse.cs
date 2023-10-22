using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.Response;

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <param name="status"></param>
    public ApiResponse(string message, T data, string status)
    {
        Message = message;
        Data = data;
        Status = status;
    }

    private ApiResponse(string message, T data, object paging, string status)
    {
        Message = message;
        Data = data;
        Pagination = paging;
        Status = status;
    }

    /// <summary>
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// </summary>
    public T Data { get; }

    /// <summary>
    /// </summary>
    public object Pagination { get; }

    /// <summary>
    /// </summary>
    public string Status { get; }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <typeparam name="TData"></typeparam>
    /// <returns></returns>
    public static ApiResponse<TData> Error<TData>(ProblemDetails message)
    {
        return new ApiResponse<TData>(message.Title, default, null, message.Status.ToString());
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="TData"></typeparam>
    /// <returns></returns>
    public static ApiResponse<TData> Ok<TData>(TData data) where TData : class
    {
        return new ApiResponse<TData>("", data, "success");
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <param name="paging"></param>
    /// <typeparam name="TData"></typeparam>
    /// <returns></returns>
    public static ApiResponse<TData> Success<TData>(TData data, object paging) where TData : class
    {
        return new ApiResponse<TData>("", data, paging, "success");
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="TData"></typeparam>
    /// <returns></returns>
    public static ApiResponse<TData> Created<TData>(TData data) where TData : class
    {
        return new ApiResponse<TData>("", data, "data modified");
    }
}