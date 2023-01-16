using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;

namespace SalveApp.Clinics.Core.Services;

public class CSVDataLoader : ICSVDataLoader
{
    //Todo: A better async alternative
    public List<T> LoadClinics<T>(string filePath)
    {
        var path = Path.Combine(filePath);
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args =>
            {
                var headerLowered = args.Header.ToLower();
                return Regex.Replace(headerLowered, "_", string.Empty);
            }
        };
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, csvConfig);
        return csv.GetRecords<T>().ToList();
    }
}