using MaterialGatePassTacker;
using Microsoft.EntityFrameworkCore;
using MaterialGatePassTracker.Services;
using MaterialGatePassTracker.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MaterialGatePassTracker.Middleware;
using MaterialGatePassTracker.DAL;
using MaterialGatePassTracker.BAL;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

//Start DB conection code
var connectionString = builder.Configuration.GetConnectionString("myconstring");

builder.Services.AddDbContext<MaterialDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<HomeBusinessLogicClass>();
builder.Services.AddScoped<AuthBusinessLogicClass>();
//End DB connection code

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// Bind email settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<EmailService>();
builder.Services.AddScoped<IStoreRepo, StoreRepo>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReportingRepo, ReportingRepo>();
builder.Services.AddScoped<IReportingService, ReportingService>();
builder.Services.AddScoped<IEntryDetailsCreationRepo, EntryDetailsCreationRepo>();
builder.Services.AddScoped<IEntryDetailsCreationService, EntryDetailsCreationService>();
builder.Services.AddScoped<HomeDataAccessLayer>();
builder.Services.AddScoped<AuthDataAccessLayer>();
builder.Services.AddScoped<HomeBusinessLogicClass>();
builder.Services.AddScoped<AuthBusinessLogicClass>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Configure the uploads folder path dynamically
string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

// Ensure the directory exists
if (!Directory.Exists(uploadsFolderPath))
{
    Directory.CreateDirectory(uploadsFolderPath);
}

// Serve static files from the uploads folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsFolderPath),
    RequestPath = "/Uploads"
});

app.UseRouting();

app.UseMiddleware<AuthorizationMiddleware>();

app.UseAuthorization();

#pragma warning disable ASP0014
// Enable MVC with controllers
app.UseEndpoints(endpoints =>
{
    // Custom route for ReportingController
    endpoints.MapControllerRoute(
        name: "reporting",
        pattern: "Reporting/Page/{pageNumber?}",
        defaults: new { controller = "Reporting", action = "Index" });

    // Default route
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
#pragma warning restore ASP0014

app.Run();
