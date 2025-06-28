using ETicaret.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSession();
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration();
builder.Services.ConfigureRouting();
builder.Services.ConfigureApplicationCookie();
builder.Services.AddMemoryCache();
builder.Services.ConfigureCsv();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseStaticFiles();
app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


// Eski kodu kaldýrýn:
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapAreaControllerRoute(
//         name: "Admin",
//         areaName: "Admin",
//         pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");
//     endpoints.MapControllerRoute(
//         name: "default",
//         pattern: "{controller=Home}/{action=Index}/{id?}");
//     endpoints.MapRazorPages();
//     endpoints.MapControllers();
// });

// Yeni top-level route registration kodunu ekleyin:
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.ConfigureAndCheckMigration();
app.ConfigureCsv();
app.ConfigureLocalization();
await app.ConfigureDefaultAdminUser();

app.Run();
