using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service2.Models;
using Service2.Repositories;
using static Common.Extensions.Constants;

[Controller]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;

    public UserController(
        ILogger<UserController> logger, 
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<User> GetUserAsync(long id)
    {
        _logger.LogInformation("Get Ok");
        using var activity = DiagnosticsConfig
            .GetSource(Service2Name)
            .StartActivity("MyActivity", ActivityKind.Internal);
        activity?.AddEvent(new ActivityEvent("my event"));
        var res = await _userRepository.GetUserByIdAsync(id);
        activity?.AddTag("MyTag", "Done");
        return res;
    }

    [HttpPost]
    public async Task<User> CreateUserAsync([FromBody] UserCreateRequest request)
    {
        var res = await _userRepository.CreateUserAsync(request);
        return res;
    }
}