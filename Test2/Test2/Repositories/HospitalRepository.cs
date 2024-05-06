using System.Data.Common;
using Microsoft.Data.SqlClient;
using Test2.Models;

namespace Test2.Repositories;

public class HospitalRepository : IHospitalRepository
{
    private readonly IConfiguration _configuration;

    public HospitalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<Patient>> GetPatientAsync(string Surname)
    {
        SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = sqlConnection;
        sqlCommand.CommandText = $"SELECT * FROM Patient WHERE LastName = @LastName";
        sqlCommand.Parameters.AddWithValue("@LastName", Surname);

        List<Patient> patients = new List<Patient>();
        
        await sqlConnection.OpenAsync();
        var reader = sqlCommand.ExecuteReader();
        while (reader.Read())
        {
            var patient = new Patient();
            patient.Id = (int)reader["Id"];
            patient.Name = (string)reader["Name"];
            patient.LastName = (string)reader["LastName"];
            patients.Add(patient);
        }

        sqlConnection.Dispose();
        sqlCommand.Dispose();

        return patients.Count == 0 ? null : patients;
    }

    public async Task<object> SomeProcedure()
    {
        SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        SqlCommand sqlCommand = new SqlCommand();
        
        await sqlConnection.OpenAsync();

        DbTransaction transaction = await sqlConnection.BeginTransactionAsync();
        sqlCommand.Transaction = (SqlTransaction)transaction;

        try
        {
            sqlCommand.CommandText = $"SELECT 1 FROM Patirnt ";
            //sqlCommand.Parameters.AddWithValue("@paremeter", parameterPassed);

            // sqlCommand.ExecuteScalar(); //To get id 
            // var reader = sqlCommand.ExecuteReader(); // to read a list of objects 
            // var affectedCount = sqlCommand.ExecuteNonQuery(); //when updating / deleting for example


            await transaction.CommitAsync();
        }
        catch (SqlException exp)
        {
            await transaction.RollbackAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
        }
        
        sqlConnection.Dispose();
        sqlCommand.Dispose();
        
        return null;
    }
}