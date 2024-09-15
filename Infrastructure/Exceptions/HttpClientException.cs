namespace remote_pokedex.Infrastructure.Exceptions;

public class HttpClientException(string error, HttpResponseErrorType ErrorType, Exception? inner = null) : Exception(error, inner)
{
    public HttpResponseErrorType ErrorType { get; } = ErrorType;
}

public enum HttpResponseErrorType
{
    FAILED,
    EMPTY
}