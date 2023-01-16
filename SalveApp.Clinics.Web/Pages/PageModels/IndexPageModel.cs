using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalveApp.Clinics.Core.Models;
using SalveApp.Clinics.Core.Services;

namespace SalveApp.Clinics.Web.Pages.PageModels;

public class IndexPageModel : PageModel
{
    private readonly IClinicsService _clinicsService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<IndexPageModel> _logger;

    public IndexPageModel(ILogger<IndexPageModel> logger, IConfiguration configuration, IClinicsService clinicsService)
    {
        _logger = logger;
        _configuration = configuration;
        _clinicsService = clinicsService;
    }

    public string Message { get; set; }
    public string NameSort { get; set; }
    public string DateOfBirthSort { get; set; }
    public string CurrentSort { get; set; }

    [BindProperty] public int? CurrentSelected { get; set; }

    [BindProperty] public int? CurrentFilter { get; set; }

    public PaginatedList<Patient> Patients { get; set; }


    public async Task<IActionResult> OnPost(string sortOrder, int? currentFilter, int pageIndex)
    {
        _logger.LogDebug(
            $"POST request sortOrder:{sortOrder}, currentFilter:{currentFilter}, pageIndex:{pageIndex} ");
        await LoadData(sortOrder, currentFilter, pageIndex);
        return Page();
    }


    public async Task<IActionResult> OnGet(string sortOrder, int? currentFilter, int? pageIndex)
    {
        _logger.LogDebug(
            $"Get request sortOrder:{sortOrder}, currentFilter:{currentFilter}, pageIndex:{pageIndex} ");
        return await LoadData(sortOrder, currentFilter, pageIndex);
    }

    private async Task<IActionResult> LoadData(string sortOrder, int? currentFilter, int? pageIndex)
    {
        var clinicsData = await GetClinicsData();

        if (CurrentSelected == null && currentFilter != null) CurrentSelected = currentFilter;

        if (CurrentSelected == null && clinicsData.Any()) CurrentSelected = clinicsData.First().Id;


        NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        DateOfBirthSort = sortOrder == "Date" ? "date_desc" : "Date";

        IEnumerable<Patient> sortable = await _clinicsService.GetPatients(CurrentSelected.Value);

        sortable = sortOrder switch
        {
            "name_desc" => sortable.OrderByDescending(s => s.LastName),
            "Date" => sortable.OrderBy(s => s.DateOfBirth),
            "date_desc" => sortable.OrderByDescending(s => s.DateOfBirth),
            _ => sortable.OrderBy(s => s.LastName)
        };

        CurrentFilter = CurrentSelected;
        CurrentSort = sortOrder;

        var pageSize = _configuration.GetValue("PageSize", 4);
        Patients = PaginatedList<Patient>.CreateAsync(sortable.ToList(), pageIndex ?? 1, pageSize);
        ViewData["ClinicData"] = new SelectList(clinicsData, "Id", "Name", CurrentSelected);

        return Page();
    }

    private async Task<List<Clinic>> GetClinicsData()
    {
        return await _clinicsService.GetAllClinics();
    }
}