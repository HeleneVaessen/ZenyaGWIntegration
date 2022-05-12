
using System.Text;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using ZenyaClient;
using ZenyaGoogle.Attributes;
using Task = System.Threading.Tasks.Task;

var builder = WebApplication.CreateBuilder(args);
var AllowAnyOrigin = "AllowAnyOrigin";
string[] Scopes = { DocsService.Scope.Documents, DocsService.Scope.Drive };
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAnyOrigin,
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    //opt.OperationFilter<GoogleHeaderAttribute>();
    opt.OperationFilter<ZenyaHeaderAttribute>();

});
builder.Services.AddScoped(x => new BaseClientService.Initializer()
{
    HttpClientInitializer = x.GetService<ITokenProvider>().GoogleCredential,
    ApplicationName = "ZenyaGoogle",
    ApiKey = "AIzaSyA9jxBxcEfEdEUSjZYVutUgOvVBoQFfm8M"
});
builder.Services.AddScoped<DriveService>();
builder.Services.AddScoped<DocsService>();

var options = new ClientOptions
{
    BaseUrl = builder.Configuration["ZenyaApi:BaseUrl"]
};
builder.Services.AddScoped<IUsersClient, UsersClient>(x =>
{
    options.Tokens = x.GetService<ITokenProvider>();
    return new UsersClient(options);
});
builder.Services.AddScoped<IDocumentsClient, DocumentsClient>(x =>
 {
     options.Tokens = x.GetService<ITokenProvider>();
     return new DocumentsClient(options);
 });
builder.Services.AddScoped<IDocumentQuickCodeClient, DocumentQuickCodeClient>(x =>
 {
     options.Tokens = x.GetService<ITokenProvider>();
     return new DocumentQuickCodeClient(options);
 });
//builder.Services.AddAuthentication(o =>
//{
//    //This forces challenge results to be handled by Google OpenID Handler, so there's no
//    // need to add an AccountController that emits challenges for Login.
//    o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
//    // This forces forbid results to be handled by Google OpenID Handler, which checks if
//    // extra scopes are required and does automatic incremental auth.
//    o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
//    // Default scheme that will handle everything else.
//    // Once a user is authenticated, the OAuth2 token info is stored in cookies.
//    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//}).AddCookie().AddGoogleOpenIdConnect(googleOptions =>
//{
//    // googleOptions.Events.OnMessageReceived = ctx =>
//    //{
//    //    var header = ctx.HttpContext.Request.Headers["X-GoogleAuth"].ToString();

//    //    if (!string.IsNullOrEmpty(header) && header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//    //    {
//    //        ctx.Token = header.Substring("Bearer ".Length).Trim();
//    //    }
//    //    return Task.CompletedTask;
//    //};
//    googleOptions.ClientId = builder.Configuration["Google:ClientId"];
//    googleOptions.ClientSecret = builder.Configuration["Google:ClientSecret"];
//});


builder.Services.AddScoped<ITokenProvider, TokenProvider>();
ConfigureClients(builder);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opt =>
    {

    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Use(async (ctx, next) =>
{
    var provider = ctx.RequestServices.GetService<ITokenProvider>();
    provider.ZenyaToken = ctx.Request.Headers["X-ZenyaAuth"];
    //provider.GoogleToken = ctx.Request.Headers["X-GoogleAuth"];
    using (var stream =
           new FileStream(
               "./Properties/client_secret_408797503550-eeprtabf7n4od01k1lbai04o7ct7av64.apps.googleusercontent.com.json",
               FileMode.Open, FileAccess.Read))
    {
        string credPath = "token.json";
        provider.GoogleCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        //Console.WriteLine("Credential file saved to: " + credPath);
    };
    await next.Invoke();
});
app.UseCors(AllowAnyOrigin);
app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureClients(WebApplicationBuilder webApplicationBuilder)
{
    ClientOptions options = new()
    {
        BaseUrl = webApplicationBuilder.Configuration["ZenyaApi:BaseUrl"]
    };


    webApplicationBuilder.Services.AddScoped<IUsersClient, UsersClient>(x =>
    {
        options.Tokens = x.GetRequiredService<ITokenProvider>();
        return new UsersClient(options);
    });
}

