namespace ZenyaClient;

public class ClientOptions
{
    public string BaseUrl { get; set; }
    public ITokenProvider Tokens { get; set; }
}