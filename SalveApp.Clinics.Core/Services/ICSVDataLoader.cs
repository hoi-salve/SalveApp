using System.Collections.Generic;

namespace SalveApp.Clinics.Core.Services;

public interface ICSVDataLoader
{
    List<T> LoadClinics<T>(string filePath);
}