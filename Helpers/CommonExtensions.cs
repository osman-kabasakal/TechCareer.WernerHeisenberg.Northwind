using System.Linq.Expressions;
using System.Reflection;
using TechCareer.WernerHeisenberg.Northwind.Attributes;
using TechCareer.WernerHeisenberg.Northwind.Models;

namespace TechCareer.WernerHeisenberg.Northwind.Helpers;

public static class CommonExtensions
{
    public static TableViewModel<T> ToTableViewModel<T>(this IQueryable<T> data, int page, int pageSize)
        where T : class, new()
    {
        return new TableViewModel<T>
        {
            Data = data.Skip(page*pageSize).Take(pageSize).ToList(),
            Page = page,
            PageSize = pageSize,
            Count = data.Count()
        };
    }
    
    public static TAttribute? GetDefinedAttribute<TAttribute>(this MemberInfo memberInfo)
        where TAttribute : Attribute
    {
        return memberInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
    }
    
    public static string GetColumnName(this MemberInfo memberInfo)
    {
        var columnNameAttribute = memberInfo.GetDefinedAttribute<ColumnNameAttribute>();
        return columnNameAttribute?.Name ?? memberInfo.Name;
    }
    
    public static MemberInfo GetMemberInfo<T>(Expression<Func<T, object>> expression)
    {
        return expression.Body switch
        {
            MemberExpression memberExpression => memberExpression.Member,
            UnaryExpression unaryExpression => ((MemberExpression)unaryExpression.Operand).Member,
            _ => throw new ArgumentException("Bilinmeyen tür.")
        };
    }
    
    public static string GetColumnName<T>(this T? _, Expression<Func<T, object>> expression)
    {
        var memberInfo = GetMemberInfo(expression);
        return memberInfo.GetColumnName();
    }
}