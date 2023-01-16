using System;
using System.Collections.Generic;
using System.Linq;

namespace SalveApp.Clinics.Web.Pages.PageModels;

public class PaginatedList<T> : List<T>
{
    private readonly int? _pageIndex;

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public int? PageIndex
    {
        get => _pageIndex;
        private init
        {
            if (value > 0)
                _pageIndex = value;
        }
    }

    public int? TotalPages { get; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> CreateAsync(
        IEnumerable<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip(
                (pageIndex - 1) * pageSize)
            .Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}