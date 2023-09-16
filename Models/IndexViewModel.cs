using TechCareer.WernerHeisenberg.Northwind.Domain.Entities;
using TechCareer.WernerHeisenberg.Northwind.Dtos;

namespace TechCareer.WernerHeisenberg.Northwind.Models;

public class IndexViewModel
{
    public IndexViewModel()
    {
        Employees = new List<EmployeeDto>();
    }
    
    public List<EmployeeDto> Employees { get; set; }
}