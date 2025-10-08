using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// CORS: laat Angular dev toe
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("http://localhost:4200")
     .AllowAnyHeader()
     .AllowAnyMethod()));

// Controllers registreren (nu of voor later)
builder.Services.AddControllers();

// OpenAPI (.NET 9 template)
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors();

// OpenAPI alleen in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// GEEN HTTPS redirection in container die alleen op 5000 draait
// app.UseHttpsRedirection();

// Eenvoudige healthcheck
app.MapGet("/health", () => Results.Ok("OK"));

// Demo minimal endpoint (houd desnoods even voor test)
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )
    );
    return Results.Ok(forecast);
});

// Zelfde endpoint met /api-prefix (handig als je frontend dat verwacht)
app.MapGet("/api/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )
    );
    return Results.Ok(forecast);
});

// Controllers (als je die straks hebt)
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
