using System.Collections;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using TechCareer.WernerHeisenberg.Northwind.Attributes;
using TechCareer.WernerHeisenberg.Northwind.Helpers;
using TechCareer.WernerHeisenberg.Northwind.Models;

namespace TechCareer.WernerHeisenberg.Northwind.HeisenbergTagHelpers;

[HtmlTargetElement("heisenberg-table", TagStructure = TagStructure.NormalOrSelfClosing)]
public class TableWithPaginationTagHelper : TagHelper
{
    private const string? COLUMN_FORMAT = "<th scope=\"col\">{0}</th>";

    private const string? THEAD_FORMAT = @"
<thead>
    <tr>
        {0}
    </tr>
</thead>
";

    private const string? TBODY_FORMAT = @"
<tbody>
    {0}
</tbody>
";

    private const string TROW_FORMAT = @"
<tr>
   {0}
</tr>
";

    private const string TDATA_FORMAT = "<td>{0}</td>";

    private const string PAGINATION_FORMAT = @"
<nav aria-label=""Page navigation example"">
            <ul class=""pagination justify-content-end"">
                <li class=""page-item {3}"">
                    <a href=""{0}"" class=""page-link"">Geri</a>
                </li>
                {1}
                <li class=""page-item {4}"">
                    <a class=""page-link"" href=""{2}"">İleri</a>
                </li>
            </ul>
        </nav>
";

    private const string PAGE_LINK_FORMAT = "<li class=\"page-item {2}\"><a class=\"page-link\" href=\"{0}\">{1}</a></li>";


    private const string CONTENT_FORMAT = @"
<div class=""col"">
<table class=""table table-striped table-hover"">
    {0}
</table>
{1}
</div>
";

    [HtmlAttributeName("data")] public ICollection Data { get; set; }
    [HtmlAttributeName("page")] public int Page { get; set; }
    [HtmlAttributeName("page-size")] public int PageSize { get; set; }
    [HtmlAttributeName("count")] public int Count { get; set; }
    
    [HtmlAttributeName("host-url")] public string HostUrl { get; set; }
    
    [HtmlAttributeName("extra-cols")] public Dictionary<string,Func<object?,string>> ExtraCols { get; set; }=new();
    
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.Add("class", "row");
        output.TagMode = TagMode.StartTagAndEndTag;

        if (Data == null)
        {
            output.SuppressOutput();
            return;
        }

        var content = string.Empty;

        var dataModel = Data.GetType().GetGenericArguments().First();
        var tableFields = dataModel.GetProperties()
            .Select((p, i) =>
            {
                var columnNameAttr = p.GetDefinedAttribute<ColumnNameAttribute>();
                var columnName = columnNameAttr?.Name ?? p.Name;

                return new
                {
                    columnName,
                    index = columnNameAttr?.Index ?? i,
                    ignore = columnNameAttr?.Ignore ?? false,
                    propertyInfo = p
                };
            }).Where(x => !x.ignore).OrderBy(p => p.index).ThenBy(x => x.columnName).ToList();

        var columns = string.Empty;

        foreach (var field in tableFields)
        {
            columns += string.Format(COLUMN_FORMAT, field.columnName);
        }

        foreach (var exCol in ExtraCols)
        {
            columns+= string.Format(COLUMN_FORMAT, exCol.Key);
        }

        var thead = string.Format(THEAD_FORMAT, columns);
        content += thead;

        var tbody = string.Empty;
        foreach (var data in Data)
        {
            var trow = string.Empty;
            foreach (var field in tableFields)
            {
                var value = field.propertyInfo.GetValue(data);
                if (value is DateTime dateValue)
                {
                    trow+=string.Format(TDATA_FORMAT, dateValue.ToString("dd.MM.yyyy"));
                    continue;
                }
                trow += string.Format(TDATA_FORMAT, value);
            }

            foreach (var exCol in ExtraCols)
            {
                var value = exCol.Value(data);
                trow += string.Format(TDATA_FORMAT, value);
            }

            tbody += string.Format(TROW_FORMAT, trow);
        }

        content += string.Format(TBODY_FORMAT, tbody);


        var pagination = string.Empty;
        var pageLinks = string.Empty;
        
        Uri.TryCreate(HostUrl, UriKind.RelativeOrAbsolute, out var hostUri);
        Uri.TryCreate(ViewContext.HttpContext.Request.GetDisplayUrl(),
            UriKind.Absolute, out var baseUri);
        Uri.TryCreate(baseUri,hostUri, out var uriBuilder);
        
        
        var uri = new UriBuilder(uriBuilder);
        var totalPages = Math.Ceiling(decimal.Divide(Count, PageSize));
        var query=HttpUtility.ParseQueryString(uri.Query);
        for (int i = 0; i < totalPages; i++)
        {
            query["page"] = i.ToString();
            query["pageSize"] = PageSize.ToString();
            uri.Query = query.ToString();
            var pageUrl = i == Page ? "javascript:void(0)" : uri.ToString();
            pageLinks += string.Format(PAGE_LINK_FORMAT, $"{pageUrl}", i + 1, i == Page ? "active" : "");
        }

        var prevPage = "javascript:void(0)";
        var prevDisabled = "disabled";
        if (Page-1>=0)
        {
            query["page"] = (Page-1).ToString();
            query["pageSize"] = PageSize.ToString();
            uri.Query = query.ToString();
            prevPage = uri.ToString();
            prevDisabled = "";
        }

        var nextPage = "javascript:void(0)";
        var nextDisabled = "disabled";
        if (totalPages > Page + 1)
        {
            query["page"] = (Page+1).ToString();
            query["pageSize"] = PageSize.ToString();
            uri.Query = query.ToString();
            nextPage = uri.ToString();
            nextDisabled = "";
        }
        
        pagination = string.Format(PAGINATION_FORMAT, prevPage, pageLinks, nextPage,prevDisabled,nextDisabled);
        
        output.Content.SetHtmlContent(string.Format(CONTENT_FORMAT, content, pagination));
    }
}