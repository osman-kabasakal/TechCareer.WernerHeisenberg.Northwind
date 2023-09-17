using TechCareer.WernerHeisenberg.Northwind.Attributes;

namespace TechCareer.WernerHeisenberg.Northwind.Dtos;

public record EmployeeOrderDto
{
    [ColumnName("Sipariş No", 0)] public int OrderId { get; set; }

    [ColumnName("Çalışan Kimliği", 1, true)]
    public int EmployeeId { get; set; }

    [ColumnName("Müşteri Kimliği", 2, true)]
    public string CustomerId { get; set; }

    [ColumnName("Sipariş Tarihi", 3)]
    public DateTime? OrderDate { get; set; }
    [ColumnName("Sipaeriş Ülkesi", 4)] 
    public string ShipCountry { get; set; }
}