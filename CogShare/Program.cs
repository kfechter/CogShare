using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;
using CogShare.EFCore;
using CogShare.EFCore.Repositories;
using CogShare.EFCore.UnitOfWork;
using CogShare.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("CogShareDBConnection");
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<CogShareContext>(options =>
                options.UseNpgsql(connectionString,
                b => b.MigrationsAssembly("CogShare.EFCore")), ServiceLifetime.Transient);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CogShareUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<CogShareContext>();

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IDocumentationRepository, DocumentationRepository>();
builder.Services.AddTransient<IExternalProjectRepository, ExternalProjectRepository>();
builder.Services.AddTransient<IHardwareRepository, HardwareRepository>();
builder.Services.AddTransient<IPersonalProjectRepository, PersonalProjectRepository>();
builder.Services.AddTransient<ISoftwareLibraryRepository, SoftwareLibraryRepository>();
builder.Services.AddTransient<ISoftwareRepository, SoftwareRepository>();

builder.Services.AddTransient<ICogShareUserRepository, CogShareUserRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<EmailSenderOptions>(builder.Configuration.GetSection("EmailConfiguration"));


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CogShareContext>();
    db.Database.Migrate();
}

app.Run();
