using TechCareer.WernerHeisenberg.Northwind.Domain.Entities;

namespace TechCareer.WernerHeisenberg.Northwind.Models;

public class IndexViewModel
{
    public IndexViewModel()
    {
        Employees = new List<Employee>();
    }
    public List<Employee> Employees { get; set; }
}