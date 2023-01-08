using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;
using totten_romatoes.Server.Data;
using totten_romatoes.Server.Services;
using totten_romatoes.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "config", "secrets.json"));

var connectionString = builder.Configuration["DefaultConnection"];
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
    {
        options.IdentityResources["openid"].UserClaims.Add("name");
        options.ApiResources.Single().UserClaims.Add("name");
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
    });

builder.Services.AddAuthentication(options =>
{
    options.RequireAuthenticatedSignIn = false;
})
    .AddIdentityServerJwt()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["google_client_id"];
        options.ClientSecret = builder.Configuration["google_client_secret"];
    })
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["microsoft_client_id"];
        options.ClientSecret = builder.Configuration["microsoft_client_secret"];
    });

builder.Services.AddAuthorization(opts =>
{

    opts.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });
});

builder.Services.AddTransient<IDropboxService, DropboxService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<ISubjectService, SubjectService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<InitializeDatabase>();

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddRazorPages();

builder.Services.AddLocalization();

var app = builder.Build();

app.Services.CreateScope().ServiceProvider.GetService<InitializeDatabase>()!.Run();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


app.Run();


