namespace PatientPortal.Infrastructure.Services;

using PatientPortal.Core.DTOs;
using PatientPortal.Core.Interfaces;
using PatientPortal.Core.Models;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<PatientResponse?> GetByIdAsync(int id)
    {
        var patient = await _repository.GetByIdAsync(id);
        return patient is null ? null : MapToResponse(patient);
    }

    public async Task<IEnumerable<PatientResponse>> GetAllAsync()
    {
        var patients = await _repository.GetAllAsync();
        return patients.Select(MapToResponse);
    }

    public async Task<PatientResponse> CreateAsync(CreatePatientRequest request)
    {
        if(request.DateOfBirth >= DateTime.UtcNow.Date)
        {
            throw new ArgumentException("Date of birth cannot be in the past.");
        }

        if (request.DateOfBirth < DateTime.UtcNow.AddYears(-130))
        {
            throw new ArgumentException("Date of birth cannot be more than 130 years in the past.");
        }
        
        var patient = new Patient
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };

        var created = await _repository.CreateAsync(patient);
        return MapToResponse(created);
    }

    private static PatientResponse MapToResponse(Patient p) => new()
    {
        Id = p.Id,
        FirstName = p.FirstName,
        LastName = p.LastName,
        DateOfBirth = p.DateOfBirth,
        PhoneNumber = p.PhoneNumber,
        Email = p.Email,
        CreatedAt = p.CreatedAt
    };
}
