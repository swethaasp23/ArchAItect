using Microsoft.AspNetCore.Mvc;
using ArchAItect.API.Services;
using ArchAItect.API.Models;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class ArchitectureController : ControllerBase
{
    private readonly ArchitectureService _service;

    public ArchitectureController(ArchitectureService service)
    {
        _service = service;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] RequestModel request)
    {
        var result = await _service.GenerateAsync(request.Requirement);

        var parsed = JsonSerializer.Deserialize<object>(result);

        return Ok(parsed);
    }
}