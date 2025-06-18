using AirTracker.Data;
using AirTracker.Repositories;
using AirTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity z domyślnym UI, rolami i EF Stores
builder.Services
    .AddDefaultIdentity<IdentityUser>(opts =>
    {
        opts.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 3. MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// 4. Rejestracja HttpClientów: OpenAQService i LocationImportService
builder.Services.AddHttpClient<IOpenAQService, OpenAQService>(client =>
{
    client.BaseAddress = new Uri("https://api.openaq.org/v3/");
    var apiKey = builder.Configuration["OpenAQ:ApiKey"];
    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    }
});

builder.Services.AddHttpClient<ILocationImportService, LocationImportService>(client =>
{
    client.BaseAddress = new Uri("https://api.openaq.org/v3/");
    var apiKey = builder.Configuration["OpenAQ:ApiKey"];
    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    }
});
builder.Services.AddSession();
// 5. Repozytoria
builder.Services.AddScoped<ISensorRepository, SensorRepository>();

var app = builder.Build();

// 6. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// 7. Endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// 8. Import lokalizacji i sensorów przy starcie
using (var scope = app.Services.CreateScope())
{
    var importer = scope.ServiceProvider.GetRequiredService<ILocationImportService>();

    Console.WriteLine(">>> START importu lokalizacji z OpenAQ");

    var cities = new[]
    {
        ("Białystok", 53.132488, 23.168840),
        ("Warszawa",  52.229676, 21.012229),
        ("Łódź",      51.759248, 19.455983),
        ("Kraków",    50.064650, 19.944980),
        ("Poznań",    52.406374, 16.925168),
        ("Gdańsk",    54.352025, 18.646638),
    };

    importer.ImportCitiesAsync(cities).GetAwaiter().GetResult();

    Console.WriteLine(">>> KONIEC importu. Sprawdź bazę.");
}

app.Run();
