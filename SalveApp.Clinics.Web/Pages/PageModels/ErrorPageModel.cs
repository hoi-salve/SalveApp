using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SalveApp.Clinics.Web.Pages.PageModels;

//Todo: Improve error handling
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorPageModel : PageModel
{
    private readonly ILogger<ErrorPageModel> _logger;

    public ErrorPageModel(ILogger<ErrorPageModel> logger)
    {
        _logger = logger;
    }

    public string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}