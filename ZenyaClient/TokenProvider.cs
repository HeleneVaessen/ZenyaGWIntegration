using Google.Apis.Auth.OAuth2;

namespace ZenyaClient;

public interface ITokenProvider
{
    [Obsolete]
    string GoogleToken { get; set; }
    string ZenyaToken { get; set; }
    UserCredential GoogleCredential { get; set; }
}

public class TokenProvider : ITokenProvider
{
    public string GoogleToken { get; set; }

    public string ZenyaToken { get; set; }
    public UserCredential GoogleCredential { get; set; }
}