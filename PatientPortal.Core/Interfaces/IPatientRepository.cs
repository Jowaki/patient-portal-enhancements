namespace PatientPortal.Core.Interfaces;

using PatientPortal.Core.Models;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(int id);
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient> CreateAsync(Patient patient);
    Task<Patient?> UpdateAsync(Patient patient);
}