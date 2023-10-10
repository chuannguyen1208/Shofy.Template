using NSwag;
using NSwag.Generation.Processors.Security;
using Shofy.Infrastructure;
using Shofy.Infrastructure.MongoDb;
using Shofy.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddUseCases()
    //.AddInfrastructure()
    .AddInfrastructureMongoDb(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(document =>
{
    document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        In = OpenApiSecurityApiKeyLocation.Header,
    });

    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

string apiPathBase = builder.Configuration["API_PATH_BASE"]!;

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

if (!string.IsNullOrWhiteSpace(apiPathBase))
{
    app.UsePathBase($"/{apiPathBase.TrimStart('/')}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseOpenApi(settings =>
{
    settings.PostProcess = (d, _) => { d.BasePath = $"/{apiPathBase.TrimStart('/')}"; };
});

app.UseSwaggerUi3();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
