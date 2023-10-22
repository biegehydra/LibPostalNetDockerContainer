using LibPostalApi.Interfaces;
using LibPostalApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILibPostalService, LibPostalService>();

builder.Services.AddHealthChecks();


var app = builder.Build();
if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
    Console.WriteLine($"Port Is Null: {string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("PORT"))}");
    builder.WebHost.UseUrls($"http://*:{port}");
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapHealthChecks("/health");
app.Use(async (context, next) =>
{
    // Could fail a health check for taking to long if it has to initialize libpostal
    if (LibPostalService.Initialized && context.Request.Path.StartsWithSegments("/health"))
    {
        var libPostal = app.Services.GetRequiredService<ILibPostalService>();
        var testParse = libPostal.ParseAddress(TestData.TestAddress);
        var testExpand = libPostal.ParseAddress(TestData.TestAddress);
        if (testParse?.ParseResults == null || testExpand?.ParseResults == null)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("LibPostalApi is not healthy");
            Console.WriteLine($"Health check endpoint LibPostalApi api is UNHEALTHY");
            return;
        }
        Console.WriteLine($"Health check endpoint called at {DateTime.UtcNow}");
    }

    await next.Invoke();
});

app.Run();

public static class TestData
{
    public static readonly List<string> TestAddress = new () { "8000 Southern Breeze Dr Orlando FL 32836" };
}