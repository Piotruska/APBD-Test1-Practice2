using Microsoft.AspNetCore.Mvc;
using Test2.Services;

namespace Test2.Controllers;

[ApiController]
[Route("[controller]")]
public class PrescriptonsController : ControllerBase
{
    private IHospitalService _service;
    
    public PrescriptonsController(IHospitalService service)
    {
        _service = service;
    }

    /// <summary>
    /// Endpoints used to .
    /// </summary>
    /// <returns>...</returns>
    /// HttpGet - Get data
    /// HttpPost - Add data
    /// HttpPut - Update Data
    /// HttpDelete - Selete Data

    [HttpPost("api/")]
    public async Task<IActionResult> Get()
    {
        //var a = await _service.GetAsync();
        return Ok();
    }
    
    
}