namespace PatientPortal.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PatientPortal.Core.Interfaces;
using PatientPortal.Core.Models;
using PatientPortal.Infrastructure.Data;

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _context;

    public PatientRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(int id)
        => await _context.Patients.FindAsync(id);

    public async Task<IEnumerable<Patient>> GetAllAsync()
        => await _context.Patients.ToListAsync();

    public async Task<Patient> CreateAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<Patient?> UpdateAsync(Patient patient)
    {
        var existing = await _context.Patients.FindAsync(patient.Id);
        if (existing is null) return null;

        existing.FirstName = patient.FirstName;
        existing.LastName = patient.LastName;
        existing.PhoneNumber = patient.PhoneNumber;
        existing.Email = patient.Email;
        await _context.SaveChangesAsync();
        return existing;
    }
}