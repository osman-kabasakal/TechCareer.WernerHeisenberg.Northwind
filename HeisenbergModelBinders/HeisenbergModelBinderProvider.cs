using Microsoft.AspNetCore.Mvc.ModelBinding;
using TechCareer.WernerHeisenberg.Northwind.Dtos;

namespace TechCareer.WernerHeisenberg.Northwind.HeisenbergModelBinders;

public class HeisenbergModelBinderProvider: IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var modelType = context.Metadata.UnderlyingOrModelType;
        if (modelType == typeof(EmployeeDto))
        {
            return new EmployeeDtoBinder();
        }

        return null;
    }
}