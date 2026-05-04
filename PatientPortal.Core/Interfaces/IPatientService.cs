namespace PatientPortal.Core.Interfaces;

using PatientPortal.Core.DTOs;

public interface IPatientService
{
    Task<PatientResponse?> GetByIdAsync(int id);
    Task<IEnumerable<PatientResponse>> GetAllAsync();
    Task<PatientResponse> CreateAsync(CreatePatientRequest request);
}
