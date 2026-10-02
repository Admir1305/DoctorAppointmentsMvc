using WebDoctor.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MVC controllers with views
builder.Services.AddControllersWithViews();

// 2. Bind ApiSettings section using the Options pattern
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// 3. Register a default HttpClient configured with the BaseUrl from appsettings.json
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
    ?? throw new InvalidOperationException("ApiSettings:BaseUrl is missing in appsettings.json");

builder.Services.AddHttpClient("DoctorApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

// 4. Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();