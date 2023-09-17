using TechCareer.WernerHeisenberg.Northwind.Domain.Entities;
using TechCareer.WernerHeisenberg.Northwind.Dtos;

namespace TechCareer.WernerHeisenberg.Northwind.Models;

public class EmployeeOrderViewModel
{
    public List<EmployeeOrderDto> Orders { get; set; }
}