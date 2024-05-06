using Test2.Models;
using Test2.Repositories;

namespace Test2.Services;

public class HospitalService : IHospitalService
{
    private IHospitalRepository _repository;

    public HospitalService(IHospitalRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Patient>> GetPatientAsync(string Surname)
    {
        return await _repository.GetPatientAsync(Surname);
    }
}