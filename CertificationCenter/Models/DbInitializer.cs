using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CertificationCenter.Models {
    public static class DbInitializer {
        public static async Task Initialize(IServiceProvider serviceProvider) {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            
            var roles = new[] {"user", "admin"};

            foreach (var roleName in roles) {
                bool rolesIsExist = await roleManager.RoleExistsAsync(roleName);

                if (!rolesIsExist) {
                    var role = new IdentityRole {
                        Name = roleName
                    };
                    await roleManager.CreateAsync(role);
                }
            }
            
            User user = await userManager.FindByNameAsync("admin");
            if (user == null) {
                User admin = new User {
                    UserName = "admin",
                    Email = "admin@email.com"
                };
                await userManager.CreateAsync(admin, "Admin1.");
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}