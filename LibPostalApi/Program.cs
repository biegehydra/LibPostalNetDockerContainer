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
    if (context.Request.Path.StartsWithSegments("/health"))
    {
        var libPostal = app.Services.GetRequiredService<ILibPostalService>();
        var testResult = libPostal.ParseAddress(TestData.TestAddress);
        if (testResult == null)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("LibPostalApi is not healthy");
            Console.WriteLine($"Health check endpoint LibPostalApi api is UNHEALTHY");
            return;
        }
        Console.WriteLine($"Health check endpoint called at {DateTime.UtcNow}");
    }

    // Call the next middleware in the pipeline
    await next.Invoke();
});

app.Run();

public static class TestData
{
    public static readonly List<string> TestAddress = new () { "8000 Southern Breeze Dr Orlando FL 32836" };
}