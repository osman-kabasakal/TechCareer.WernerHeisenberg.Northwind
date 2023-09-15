using TechCareer.WernerHeisenberg.Northwind.Domain.Entities;

namespace TechCareer.WernerHeisenberg.Northwind.Models;

public class EmployeeOrderViewModel
{
    public Employee Employee { get; set; }
    public List<Order> Orders { get; set; }
}