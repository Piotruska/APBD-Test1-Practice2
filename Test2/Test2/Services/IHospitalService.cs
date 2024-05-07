using Test2.Models;
using Test2.Models.DTOs;

namespace Test2.Services;

public interface IHospitalService
{
    public Task<List<Patient>> GetPatientAsync(string Surname);

    public Task<string> AddNewPrescritpion(Prescription dto);
}