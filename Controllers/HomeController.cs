using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechCareer.WernerHeisenberg.Northwind.Domain.Context;
using TechCareer.WernerHeisenberg.Northwind.Dtos;
using TechCareer.WernerHeisenberg.Northwind.Models;

namespace TechCareer.WernerHeisenberg.Northwind.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NorthwindContext _northwindContext;

    public HomeController(ILogger<HomeController> logger, NorthwindContext northwindContext)
    {
        _logger = logger;
        _northwindContext = northwindContext;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index([FromQuery]int page=0,[FromQuery]int pageSize=50)
    {
        var employees = _northwindContext.Employees.AsNoTracking()
            .Select(employee =>
                new EmployeeDto()
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Title = employee.Title,
                    EmployeeId = employee.EmployeeId,
                })
            .Skip(page * pageSize).Take(pageSize).ToList();
        
        return View(new IndexViewModel()
        {
            Employees = employees
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet("{employeeId}")]
    public IActionResult EmployeeOrder(int employeeId)
    {
        
        return View();
    }
}
