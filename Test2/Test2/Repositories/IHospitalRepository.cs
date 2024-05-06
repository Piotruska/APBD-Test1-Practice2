using Test2.Models;

namespace Test2.Repositories;

public interface IHospitalRepository
{
    public Task<List<Patient>> GetPatientAsync(string Surname);
    
    public Task<Object> SomeProcedure();
}