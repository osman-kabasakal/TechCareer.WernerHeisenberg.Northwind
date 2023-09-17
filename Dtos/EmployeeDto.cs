using TechCareer.WernerHeisenberg.Northwind.Attributes;

namespace TechCareer.WernerHeisenberg.Northwind.Dtos;

public record EmployeeDto
{
    [ColumnName("#",0)]
    public int EmployeeId { get; set; }
    [ColumnName("Adı",1)]
    public string FirstName { get; set; }
    [ColumnName("Soyadı",2)]
    public string LastName { get; set; }
    [ColumnName("Ünvanı",3)]
    public string Title { get; set; }
}