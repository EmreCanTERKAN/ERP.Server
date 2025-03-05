using ERP.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ERP.Server.WebAPI;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                {
                    // Id = Guid.Parse("e4931370-d0aa-47fc-8af4-11a4cbc7732b"),
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "EmreCan",
                    LastName = "TERKAN",
                    EmailConfirmed = true,
                    CreateAt = DateTimeOffset.Now,
                };

                user.CreateUserId = user.Id;

                userManager.CreateAsync(user, "1").Wait();


            }
        }
    }
}
