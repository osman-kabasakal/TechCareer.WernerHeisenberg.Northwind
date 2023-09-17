using System.Collections;

namespace TechCareer.WernerHeisenberg.Northwind.Models;

public class TableViewModel<T>: ITableViewModel
{
    public TableViewModel()
    {
        Data = new List<T>();
    }

    public List<T> Data { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
}

public interface ITableViewModel
{
    int Page { get; set; }
    int PageSize { get; set; }
    int Count { get; set; }
}