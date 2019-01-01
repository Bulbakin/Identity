using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Role[] roles = (Role[])Enum.GetValues(typeof(Role));

                foreach(var r in roles)
                {
                    var identityRole = new IdentityRole
                    {
                        Id = r.GetRoleName(),
                        Name = r.GetRoleName()
                    };

                    if(!await roleManager.RoleExistsAsync(roleName: identityRole.Name))
                    {
                        var result = await roleManager.CreateAsync(identityRole);
                        if (!result.Succeeded)
                        {
                            throw new Exception("Creating role failed.");
                        }
                    }
                }

                ApplicationUser user = new ApplicationUser
                {
                    firstName = "Jane",
                    lastName = "Doe",
                    Email = "janedoe@example.com",
                    UserName = "janedoe@example.com",
                    LockoutEnabled = false
                };

                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = await userManager.CreateAsync(user, password: "5ESTdYB5cyYwA2dKhJqyjPYnKUc&45Ydw^gz^jy&FCV3gxpmDPdaDmxpMkhpp&9TRadU%wQ2TUge!TsYXsh77Qmauan3PEG8!6EP");

                    if (!result.Succeeded)
                    {
                        throw new Exception("Creating user failed");
                    }

                    result = await userManager.AddToRolesAsync(user, roles.Select(r => r.GetRoleName()));

                    if (!result.Succeeded)
                    {
                        throw new Exception("Adding user to role failed");
                    }
                }
            }

            host.Run();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
