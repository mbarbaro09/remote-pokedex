namespace remote_pokedex.Infrastructure.Exceptions;

public class HttpClientException(string error, Exception? inner = null) : Exception(error, inner);
