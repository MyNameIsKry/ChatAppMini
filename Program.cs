using ChatAppMini.Data;
using ChatAppMini.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "ChatAppMini API";
    config.Title = "ChatApp Mini API";
    config.Version = "v1";
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi(config => {
    config.DocumentTitle = "ChatApp Mini API";
    config.Path = "/swagger";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
});

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

app.MapGet("/", () => "Hello World!");
app.Run();
