using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using TechCareer.WernerHeisenberg.Northwind.Domain.Context;
using TechCareer.WernerHeisenberg.Northwind.HeisenbergModelBinders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new HeisenbergModelBinderProvider());
});
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddDbContext<NorthwindContext>(
    (options) => { options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnectionString")); },
    ServiceLifetime.Scoped);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();