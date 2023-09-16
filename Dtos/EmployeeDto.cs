namespace TechCareer.WernerHeisenberg.Northwind.Dtos;

public record EmployeeDto
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
}