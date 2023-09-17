using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechCareer.WernerHeisenberg.Northwind.Domain.Context;
using TechCareer.WernerHeisenberg.Northwind.Domain.Entities;
using TechCareer.WernerHeisenberg.Northwind.Dtos;
using TechCareer.WernerHeisenberg.Northwind.Helpers;
using TechCareer.WernerHeisenberg.Northwind.Models;
using TechCareer.WernerHeisenberg.Northwind.Validations;

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
    public IActionResult Index([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
    {
        var query = _northwindContext.Employees.AsNoTracking()
            .Select(employee =>
                new EmployeeDto()
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Title = employee.Title,
                    EmployeeId = employee.EmployeeId,
                }).OrderByDescending(x => x.EmployeeId);

        return View(query.ToTableViewModel(page,pageSize));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet("[action]/{employeeId}")]
    public IActionResult EmployeeOrders(int employeeId, [FromQuery] int page = 0, [FromQuery] int pageSize = 10)
    {
        var query = from employee in _northwindContext.Employees.AsNoTracking()
            join order in _northwindContext.Orders.AsNoTracking() on employee.EmployeeId equals order.EmployeeId
            where employee.EmployeeId == employeeId
            select new EmployeeOrderDto()
            {
                OrderId = order.OrderId,
                EmployeeId = employee.EmployeeId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                ShipCountry = order.ShipCountry
            };

        return View(query.ToTableViewModel(page,pageSize));
    }

    [HttpGet("[action]")]
    public IActionResult InsertEmployee()
    {
        return View();
    }

    [HttpPost("[action]")]
    public IActionResult InsertEmployee([FromForm] EmployeeDto employeeDto)
    {
        var validator = new EmployeeValidator();
        var validationResult = validator.Validate(employeeDto);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            
            return View(employeeDto);
        }
        
        var employee = new Employee()
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Title = employeeDto.Title
        };
        _northwindContext.Employees.Add(employee);
        _northwindContext.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
}