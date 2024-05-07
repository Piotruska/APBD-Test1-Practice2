using Microsoft.AspNetCore.Mvc;
using Test2.Models.DTOs;
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

    [HttpPost("/api/prescriptons")]
    public async Task<IActionResult> AddNewPrescritpion(Prescription dto)
    {
        var a = await _service.AddNewPrescritpion(dto);
        switch (a)
        {
           case "404D":
               return NotFound("Doctor not Found");
           case "404P":
               return NotFound("Patient not Found");
           case "400":
               return BadRequest("Amount is has to be biger then 0");
               
        }
        return Ok("Prescription was added");
    }
    
    
}