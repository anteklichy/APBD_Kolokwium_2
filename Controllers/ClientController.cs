using Kolokwium2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium2.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientWithSubscriptions(int id)
    {
        var client = await _clientService.GetClientWithSubscriptionsAsync(id);
        return Ok(client);
    }
}
