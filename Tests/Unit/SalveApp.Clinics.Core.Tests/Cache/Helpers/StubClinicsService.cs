using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalveApp.Clinics.Core.Models;
using SalveApp.Clinics.Core.Services;

namespace SalveApp.Clinics.Core.Tests.Cache.Helpers;

public class StubClinicService : IClinicsService
{
    public Task<List<Clinic>> GetAllClinics()
    {
        return Task.FromResult(DummyData.Clinics().ToList());
    }

    public Task<List<Patient>> GetPatients(int clinicId)
    {
        return Task.FromResult(DummyData.Patients(clinicId).ToList());
    }
}