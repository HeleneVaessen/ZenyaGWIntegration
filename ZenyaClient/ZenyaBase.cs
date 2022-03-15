using Newtonsoft.Json;

namespace ZenyaClient;

public class ZenyaBase
{
    private ClientOptions configuration;
    protected string BaseUrl;

    public ZenyaBase(ClientOptions configuration)
    {
        this.configuration = configuration;
        BaseUrl = configuration.BaseUrl;
    }

    protected async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
    {
        
        return new HttpRequestMessage();
    }
    /// <summary>
    /// Custom json Settings
    /// </summary>
    /// <param name="settings"></param>
    protected void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.DateFormatString = "yyyyMMddHHmmss"; //zenya uses stupid datetime format
    }


}