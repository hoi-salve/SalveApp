using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SalveApp.Clinics.Core.Models;

namespace SalveApp.Clinics.Core.Services;

// <summary>
// 1. Ideally CSVDataLoader will be running as part of a background service
// 2. dumping results into a db storage
// 3. this service will be loading from that storage
// 4. and will be caching for the configured period of time
// 5. before serving to the UI
// </summary>
public class ClinicsService : IClinicsService
{
    private readonly ICSVDataLoader _csvDataLoader;
    private readonly ILogger<ClinicsService> _logger;

    public ClinicsService(ILogger<ClinicsService> logger, ICSVDataLoader csvDataLoader)
    {
        _logger = logger;
        _csvDataLoader = csvDataLoader;
    }

    public async Task<List<Clinic>> GetAllClinics()
    {
        return LoadFromCSV<Clinic>("clinics");
    }

    public async Task<List<Patient>> GetPatients(int clinicId)
    {
        return LoadFromCSV<Patient>("patient")
            .Where(p => p.ClinicId == clinicId)
            .ToList();
    }

    private List<T> LoadFromCSV<T>(string fileType)
    {
        _logger.LogDebug($"Getting {fileType} data from csv files");
        // Todo: Read from the configs
        var files = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}CSVFiles\\", $"{fileType}*.csv");
        var list = files.Select(s => _csvDataLoader.LoadClinics<T>(s)).ToList();
        return list.SelectMany(c => c).ToList();
    }
}