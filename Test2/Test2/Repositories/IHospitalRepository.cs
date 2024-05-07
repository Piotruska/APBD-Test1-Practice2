using Test2.Models;
using Test2.Models.DTOs;

namespace Test2.Repositories;

public interface IHospitalRepository
{
    public Task<List<Patient>> GetPatientAsync(string Surname);
    
    public Task<string> AddNewPrescription(Prescription dto);
}