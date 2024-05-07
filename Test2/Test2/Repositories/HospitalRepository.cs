using System.Data.Common;
using Microsoft.Data.SqlClient;
using Test2.Models;
using Test2.Models.DTOs;

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

    public async Task<string> AddNewPrescription(Prescription dto)
    {
        SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = sqlConnection;
        
        await sqlConnection.OpenAsync();

        DbTransaction transaction = await sqlConnection.BeginTransactionAsync();
        sqlCommand.Transaction = (SqlTransaction)transaction;

        try
        {
            sqlCommand.CommandText = $"SELECT count(*) FROM Patient WHERE Id = @parientID ";
            sqlCommand.Parameters.AddWithValue("@parientID", dto.patientId);
            var count = (int) sqlCommand.ExecuteScalar();
            if (count != 1)
            {
                throw new Exception("404P");
            }
            
            sqlCommand.CommandText = $"SELECT count(*) FROM Doctor WHERE Id = @doctorID ";
            sqlCommand.Parameters.AddWithValue("@doctorID", dto.doctorId);
            count = (int) sqlCommand.ExecuteScalar();
            if (count != 1)
            {
                throw new Exception("404D");
            }

            int medicationID = 0;
            sqlCommand.CommandText = $"SELECT Id FROM Medicine WHERE Name = @medicine ";
            sqlCommand.Parameters.AddWithValue("@medicine", dto.medicine);
            var responce =  sqlCommand.ExecuteScalar();
            
            if (responce == null || responce == DBNull.Value)
            {
                sqlCommand.CommandText = $"Insert into Medicine(name) values (@medicine); " +
                                         $"SELECT SCOPE_IDENTITY();";
                var a = sqlCommand.ExecuteScalar();
                medicationID = Convert.ToInt32(a);
            }
            else
            {
                medicationID = Convert.ToInt32(responce);
            }
            
            sqlCommand.CommandText = $"Insert into Prescription(medicine_id, patient_id, doctor_id, amount, createdat)" +
                                     $" values (@medicineID,@parientID,@doctorID,@amount,@date)";
            sqlCommand.Parameters.AddWithValue("@medicineID", medicationID);
            sqlCommand.Parameters.AddWithValue("@amount", dto.amount);
            sqlCommand.Parameters.AddWithValue("@date", DateTime.Now);
            
            sqlCommand.ExecuteNonQuery();
            

            // sqlCommand.ExecuteScalar(); //To get id ro count
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
            return e.Message;
        }
        
        sqlConnection.Dispose();
        sqlCommand.Dispose();
        
        return null;
    }
}