using NSwag;
using NSwag.Generation.Processors.Security;
using Shofy.UseCases;
using Microsoft.Extensions.FileProviders;
#if (UseEfCore)
using Shofy.Infrastructure.EfCore;
#elif (UseMongoDb)
using Shofy.Infrastructure.MongoDb;
#else
using Shofy.Infrastructure;
#endif

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SqlConnection")!;

// Add services to the container.
builder.Services
    .AddUseCases()
#if (UseEfCore)
    .AddInfrastructureEfCore(connectionString);
#elif (UseMongoDb)
    .AddInfrastructureMongoDb(builder.Configuration);
#else
    .AddInfrastructure();
#endif


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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
    RequestPath = "/StaticFiles"
});


app.UseOpenApi(settings =>
{
    settings.PostProcess = (d, _) => { d.BasePath = $"/{apiPathBase.TrimStart('/')}"; };
});

app.UseSwaggerUi3();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
