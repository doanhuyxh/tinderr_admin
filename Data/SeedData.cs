using Microsoft.AspNetCore.Identity;
using tinderr.Models;

namespace tinderr.Data
{
    public class SeedData
    {
        public interface IIdentityDataInitializer
        {
            Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager);
        }

        public class IdentityDataInitializer : IIdentityDataInitializer
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IConfiguration _configuration;

            public IdentityDataInitializer(
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager,
                IConfiguration configuration)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _configuration = configuration;
            }

            public async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {

                // Add roles          
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Member"));

                // Add super admin user
                var superAdminEmail = _configuration["SuperAdminDefaultOption:Email"];
                var superAdminUserName = _configuration["SuperAdminDefaultOption:Username"];
                var superAdminPassword = _configuration["SuperAdminDefaultOption:Password"];
                var superAdminUser = new ApplicationUser
                {
                    Email = superAdminEmail,
                    UserName = superAdminUserName,
                    Name = "SuperAdmin",
                    IsActive = true,
                    AvatartPath = "/upload/avatar/blank_avatar.png",
                    InviteCode = "99999",
                    InvitedCount = 0,
                    CountWatch = 2,
                    Balance = 0,
                    LastLogin = DateTime.Now,
                    CreateDate = DateTime.Now,
                    Bankname = "-----",
                    Banknumber = "-------"

                };

                var result1 = await userManager.CreateAsync(superAdminUser, superAdminPassword);

                if (result1.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }
            }
        }

    }
}
