using FluentValidation;
using TechCareer.WernerHeisenberg.Northwind.Dtos;
using TechCareer.WernerHeisenberg.Northwind.Helpers;

namespace TechCareer.WernerHeisenberg.Northwind.Validations;

public class EmployeeValidator: AbstractValidator<EmployeeDto>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.EmployeeId).Empty().WithMessage("EmployeeId boş olmalı.");
        
        RuleFor(x => x.FirstName).NotEmpty()
            .WithMessage((validationObj)=>$"{validationObj.GetColumnName(x=>x.FirstName)} boş olamaz.")
            .MaximumLength(50)
            .WithMessage((validationObj)=>$"{validationObj.GetColumnName(x=>x.FirstName)} 50 karakterden fazla olamaz.");
        
        RuleFor(x => x.LastName).NotEmpty()
            .WithMessage((validationObj)=>$"{validationObj.GetColumnName(x=>x.LastName)} boş olamaz.")
            .MaximumLength(50)
            .WithMessage((validationObj)=>$"{validationObj.GetColumnName(x=>x.LastName)} 50 karakterden fazla olamaz.");
        
        RuleFor(x => x.Title).NotEmpty()
            .WithMessage((validationObj)=>$"{validationObj.GetColumnName(x=>x.Title)} boş olamaz.")
            .MaximumLength(50)
            .WithMessage((validationObj)=>$"{validationObj.GetColumnName(x=>x.Title)} 50 karakterden fazla olamaz.");
    }
}