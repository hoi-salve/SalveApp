using System.Collections.Generic;
using System.Threading.Tasks;
using SalveApp.Clinics.Core.Models;

namespace SalveApp.Clinics.Core.Services;

public interface IClinicsService
{
    Task<List<Clinic>> GetAllClinics();
    Task<List<Patient>> GetPatients(int clinicId);
}