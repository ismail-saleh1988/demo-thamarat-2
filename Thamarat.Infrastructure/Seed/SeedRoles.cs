//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection;

//public static class SeedRoles
//{
//    public static async Task SeedAsync(IServiceProvider serviceProvider)
//    {
//        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

//        // 1. إنشاء صلاحيات
//        string[] roles = { "Admin", "User" };
//        foreach (var role in roles)
//        {
//            if (!await roleManager.RoleExistsAsync(role))
//                await roleManager.CreateAsync(new IdentityRole(role));
//        }

//        // 2. إنشاء مستخدم أدمن
//        string adminEmail = "admin@thamarat.com";
//        string password = "Admin@123";

//        if (await userManager.FindByEmailAsync(adminEmail) == null)
//        {
//            var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
//            var result = await userManager.CreateAsync(admin, password);
//            if (result.Succeeded)
//            {
//                await userManager.AddToRoleAsync(admin, "Admin");
//            }
//        }
//    }
//}

//using Microsoft.AspNetCore.Identity;

//namespace Thamarat.Web.Data
//{
//    public static class SeedRoles
//    {
//        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
//        {
//            string[] roleNames = { "Admin", "User" ,"Accountant" , "Manager" };

//            foreach (var role in roleNames)
//            {
//                if (!await roleManager.RoleExistsAsync(role))
//                {
//                    await roleManager.CreateAsync(new IdentityRole(role));
//                }
//            }
//        }
//    }
//}
using Microsoft.AspNetCore.Identity;
namespace Thamarat.Web.Data
{
    public static class SeedRoles
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User", "Accountant", "Manager" };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }

}

