namespace Test2.Models.DTOs;

public class Prescription
{
    public int doctorId { get; set; }
    public int patientId { get; set; }
    public string medicine { get; set; }
    public int amount { get; set; }
}