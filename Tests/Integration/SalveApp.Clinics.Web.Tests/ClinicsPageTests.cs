using System;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using SalveApp.Clinics.Core.Models;
using SalveApp.Clinics.Web.Tests.Helpers;
using Xunit;

namespace SalveApp.Clinics.Web.Tests;

public class ClinicsPageTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _clinicSearchClient;

    public ClinicsPageTests(CustomWebApplicationFactory<Startup> factory)
    {
        factory.ClientOptions.BaseAddress = new Uri("http://localhost/");
        _clinicSearchClient = factory.CreateClient();
    }

    [Fact]
    public async Task When_loading_home_page_then_web_page_contains_the_2nd_clinics_into_the_dropdown()
    {
        // Arrange 
        using var pageContent = await GetDocumentAsync(await _clinicSearchClient.GetAsync("/"));

        // Act
        var queryInput = GetDropdown(await GetForm(pageContent));

        // Assert
        Assert.NotNull(queryInput);
        var options = queryInput.Options;
        Assert.NotNull(options);
        Assert.True(options.Length == 2);
        Assert.True(options[0].IsSelected);
        Assert.True(options[0].Text == "Salve Fertility");
        Assert.True(options[1].Text == "London IVF");
    }

    [Fact]
    public async Task When_loading_home_page_then_web_page_contains_the_1st_clinics_patients_data()
    {
        // Arrange
        var patient = new Patient
        {
            FirstName = "Agate",
            LastName = "Danny",
            DateOfBirth = DateTime.Parse("1969-01-31")
        };

        // Act
        var response = await _clinicSearchClient.GetAsync("/");

        // Assert
        await AssertPatientRecord(response, patient);
    }

    [Fact]
    public async Task
        When_changing_the_clinics_dropdown_selection_then_web_page_contains_the_2nd_clinics_patients_data()
    {
        // Arrange
        var patient = new Patient
        {
            FirstName = "Ackerley",
            LastName = "Mitchel",
            DateOfBirth = DateTime.Parse("1980-07-01")
        };
        using var pageContent = await GetDocumentAsync(await _clinicSearchClient.GetAsync("/"));

        var form = await GetForm(pageContent);
        var queryInput = GetDropdown(form);
        if (queryInput != null) queryInput.Value = "2";


        // Act
        var responseMessage = await _clinicSearchClient.SendAsync(form, queryInput);

        // Assert
        await AssertPatientRecord(responseMessage, patient);
    }

    private static async Task<IDocument> GetDocumentAsync(HttpResponseMessage response)
    {
        var contentStream = await response.Content.ReadAsStreamAsync();

        var browser = BrowsingContext.New();

        var document = await browser.OpenAsync(virtualResponse =>
        {
            virtualResponse.Content(contentStream, true);
            virtualResponse.Address(response.RequestMessage?.RequestUri).Status(response.StatusCode);
        });

        return document;
    }

    private static IHtmlSelectElement GetDropdown(IParentNode form)
    {
        var queryInput = form.QuerySelector("#CurrentSelected") as IHtmlSelectElement;
        return queryInput;
    }

    private static async Task AssertPatientRecord(HttpResponseMessage response, Patient patient)
    {
        using var content = await GetDocumentAsync(response);

        var cells = content.QuerySelector<IHtmlTableElement>("table")?.Rows[1].Cells;

        Assert.True(cells[0].TextContent.Contains(patient.FirstName), "Patient's FirstName is not found");
        Assert.True(cells[1].TextContent.Contains(patient.LastName), "Patient's LastName is not found");
        Assert.True(cells[2].TextContent.Contains(patient.DateOfBirth.ToString()), "Patient DateOfBirth is not found");
    }

    private static Task<IHtmlFormElement> GetForm(IParentNode pageContent)
    {
        var form = pageContent.QuerySelector<IHtmlFormElement>("form");
        return Task.FromResult(form);
    }
}