using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.ViewModels;
public class PagedViewModel<T> : IPagedList where T : class
{
    private PagedViewModel(List<T> items, int page, int pageSize, int totalCount, string reference, string? search, string? sortColumn, string? sortOrder)
    {
        Items = items;
        PageIndex = page;
        PageSize = pageSize;
        TotalItems = totalCount;
        Action = reference;
        Search = search;
        SortColumn = sortColumn;
        SortOrder = sortOrder;
    }

    public string Action { get; } = string.Empty;
    public ICollection<T> Items { get; } = [];
    public int PageIndex { get; }
    public int PageSize { get; }
    public string? Search { get; } = string.Empty;
    public string? SortColumn { get; } = string.Empty;
    public string? SortOrder { get; } = string.Empty;
    public int TotalItems { get; }
    public double TotalPages => Math.Ceiling((double)TotalItems / PageSize);
    public int PrevPage => PageIndex - 1;
    public int NextPage => PageIndex + 1;
    public double MaxLeft => CalculteMaxLeft();
    public double MaxRight => CalculteMaxRight();
    public bool HasNextPage => PageIndex * PageSize < TotalItems;
    public bool HasPreviousPage => PageIndex > 1;

    private double CalculteMaxLeft()
    {
        var maxLeft = PageIndex - Math.Floor(7.0 / 2);
        if (maxLeft < 1) maxLeft = 1;

        return maxLeft;
    }

    private double CalculteMaxRight()
    {
        var maxRight = PageIndex + Math.Floor(7.0 / 2);
        if (maxRight > TotalPages) maxRight = TotalPages;

        return maxRight;
    }

    public static async Task<PagedViewModel<T>> CreateAsync(IQueryable<T> query, int page, int pageSize, string reference, string? search, string? sortColumn, string? sortOrder)
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new(items, page, pageSize, totalCount, reference, search, sortColumn, sortOrder);
    }
}

public interface IPagedList
{
    public string Action { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public string? Search { get; }
    public string? SortColumn { get; }
    public string? SortOrder { get; }
    public int TotalItems { get; }
    public double TotalPages { get; }
    public int PrevPage { get; }
    public int NextPage { get; }
    public double MaxLeft { get; }
    public double MaxRight { get; }
    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }

}