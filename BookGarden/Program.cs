using BookGarden.Data;
using BookGarden.Services;
using BookGardenDataAccess.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SQLConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("BookGarden"));
});

var app = builder.Build();

// Apply pending schema changes before any request can read Category.DisplayOrder.
// This is required because the current model expects DisplayOrder to be an int,
// while older databases may still have the original nvarchar column.
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

// Store uploaded images in the existing web root image directory. The project
// contains a file named "Images" at the content root, so that path cannot be
// used as a directory.
var imagesPath = Path.Combine(builder.Environment.WebRootPath!, "images");
Directory.CreateDirectory(imagesPath);

// Serve wwwroot files, including product images uploaded at runtime, using
// their normal lowercase URLs such as /images/products/book.png.
app.UseStaticFiles();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();