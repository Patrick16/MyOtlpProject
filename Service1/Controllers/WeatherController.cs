using Microsoft.AspNetCore.Mvc;

[Controller]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    List<string> summaries = new List<string> { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
    private readonly ILogger<WeatherController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherController(ILogger<WeatherController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeather()
    {
        WeatherForecast[] forecast;
        // using var activity1 = DiagnosticsConfig.Source.StartActivity("Before", ActivityKind.Internal);
        // {
        _logger.LogInformation("Get weather forecast");
        forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Count)]
            ))
            .ToArray();

        _logger.LogInformation("Weather forecast: {@Response}", forecast);
        _logger.LogInformation("Call another service");
        // }
        // using var activity2 = DiagnosticsConfig.Source.StartActivity("Call", ActivityKind.Internal);
        // {

        var client = _httpClientFactory.CreateClient("myapp2");
        var user = await client.GetAsync("user/1");
        // }
        //using var activity3 = DiagnosticsConfig.GetSource(Service1Name).StartActivity("After", ActivityKind.Internal);
        //{
            _logger.LogInformation("Another service was called");
        //}
        var u = await user.Content.ReadAsStringAsync();
        return Ok(new
        {
            User = u,
            forecast = forecast
        });
    }
}
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}