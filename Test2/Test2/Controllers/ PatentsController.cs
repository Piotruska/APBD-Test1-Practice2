using Microsoft.AspNetCore.Mvc;
using Test2.Services;

namespace Test2.Controllers;

[ApiController]
[Route("[controller]")]
public class  PatentsController : ControllerBase
{
    private IHospitalService _service;
    
    public  PatentsController(IHospitalService service)
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

    [HttpPost("/api/patents{Surname:required}")]
    public async Task<IActionResult> Get(string Surname)
    {
        var a = await _service.GetPatientAsync(Surname);
        if (a == null)
        {
            return NoContent();
        }
        return Ok(a);
    }
    
    
}