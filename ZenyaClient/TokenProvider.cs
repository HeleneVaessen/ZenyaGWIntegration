namespace ZenyaClient;

public interface ITokenProvider
{
    string GoogleToken { get; set; }
    string ZenyaToken { get; set; }
}

public class TokenProvider : ITokenProvider
{
    public string GoogleToken { get; set; }

    public string ZenyaToken { get; set; }
}