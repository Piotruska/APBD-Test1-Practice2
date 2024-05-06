using Test2.Models;

namespace Test2.Services;

public interface IHospitalService
{
    public Task<List<Patient>> GetPatientAsync(string Surname);
}