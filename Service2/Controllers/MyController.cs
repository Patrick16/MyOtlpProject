using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[Controller]
[Route("[controller]")]
public class MyController : ControllerBase
{
    private readonly ILogger<MyController> _logger;

    public MyController(ILogger<MyController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetOK()
    {
        _logger.LogInformation("Get Ok");
        using var activity = DiagnosticsConfig.Source.StartActivity("MyActivity", ActivityKind.Internal);
        activity?.AddEvent(new ActivityEvent("my event"));
        await Task.Delay(1000);
        activity?.AddTag("MyTag", "Done");
        return Ok();
    }
}