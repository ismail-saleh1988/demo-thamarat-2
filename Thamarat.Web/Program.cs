using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using Thamarat.Domain.Interfaces;
using Thamarat.Infrastructure.Repositories;
using Thamarat.Infrastructure.Persistence;
using Thamarat.Web.Data;
using Microsoft.AspNetCore.Mvc.Razor;


namespace Thamarat.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ 1. إعداد EF Core
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Thamarat.Infrastructure") // ← اسم المشروع اللي فيه DbContext
    )
);


            // ✅ 2. إعداد الهوية Identity
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // ✅ 3. Razor Pages + Localization
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            builder.Services.AddRazorPages();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("ar"),
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture("ar");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // اختياري: دعم query string ?culture=en
                options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            });

            // ✅ 4. Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // ✅ 5. Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ✅ 6. تفعيل اللغة
            var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseAuthentication();
            app.UseAuthorization();

            // ✅ 7. Routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            // ✅ 8. إنشاء الأدوار
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                Thamarat.Web.Data.SeedRoles.InitializeAsync(roleManager).GetAwaiter().GetResult();
            }

            app.Run();
        }
    }
}
