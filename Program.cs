using EventRegistrationSystem.Areas.Identity.Models;
using EventRegistrationSystem.Data;
using EventRegistrationSystem.Filters;
using EventRegistrationSystem.Middlewares;
using EventRegistrationSystem.Repositories;
using EventRegistrationSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EventRegistrationSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventRegistrationSystemDb") ?? throw new InvalidOperationException("Connection string 'EventRegistrationSystemDb' not found.")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EventRegistrationSystemDbContext>();

builder.Services.AddIdentityCore<GeneralUser>().AddEntityFrameworkStores<EventRegistrationSystemDbContext>();
builder.Services.AddIdentityCore<OrganizationUser>().AddEntityFrameworkStores<EventRegistrationSystemDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(3); // authentication ticket becomes invalid after 3 days
    options.LoginPath = "/login";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();
builder.Services.AddScoped<IEventEnrollmentService, EventEnrollmentService>();
builder.Services.AddScoped<IEventImageService, EventImageService>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
builder.Services.AddScoped<IEventEnrollmentRepository, EventEnrollmentRepository>();
builder.Services.AddScoped<IEventImageRepository, EventImageRepository>();

// register below filters to use ServiceFilter
builder.Services.AddScoped<ValidateEventExistsFilter>();
builder.Services.AddScoped<AuthorizeEventFilter>();
builder.Services.AddScoped<RedirectAuthenticatedUserFilter>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // find all profiles

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // create database and tables
    var dbContext = scope.ServiceProvider.GetRequiredService<EventRegistrationSystemDbContext>();
    dbContext.Database.Migrate();

    // create roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("GeneralUser"))
        await roleManager.CreateAsync(new IdentityRole("GeneralUser"));
    if (!await roleManager.RoleExistsAsync("OrganizationUser"))
        await roleManager.CreateAsync(new IdentityRole("OrganizationUser"));

    // initialize event categories
    var eventCategoryService = scope.ServiceProvider.GetRequiredService<IEventCategoryService>();
    await eventCategoryService.InitializeCategoriesAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// add the custom middleware to the HTTP request pipeline
app.UseJqueryAjaxApiRequest();

app.UseSession(); // enable session middleware

app.MapControllerRoute(
    name: "organizationDetails",
    pattern: "organizations/{organizationName}",
    defaults: new { controller = "Organizations", action = "Details" });
app.MapControllerRoute(
    name: "eventDetails",
    pattern: "events/{id:int}",
    defaults: new { controller = "Events", action = "Details" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
