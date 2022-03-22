using ZenyaClient;
using ZenyaGoogle.Attributes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
      opt.OperationFilter<GoogleHeaderAttribute>();
    opt.OperationFilter<ZenyaHeaderAttribute>();

});
var options = new ClientOptions
{
    BaseUrl = builder.Configuration["ZenyaApi:BaseUrl"]
};
builder.Services.AddScoped<IUsersClient, UsersClient>(x =>
{
    options.Tokens = x.GetService<ITokenProvider>();
    return new UsersClient(options);
});
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

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
    provider.GoogleToken = ctx.Request.Headers["X-GoogleAuth"];
    await next.Invoke();
});

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

