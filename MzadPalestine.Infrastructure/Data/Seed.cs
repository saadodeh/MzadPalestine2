using Microsoft.AspNetCore.Identity;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Infrastructure.Data;

public static class Seed
{
    public static async Task SeedData(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            await SeedRoles(roleManager);
        }

        if (!userManager.Users.Any())
        {
            await SeedUsers(userManager);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "User", "Moderator" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin@mzadpalestine.com",
            Email = "admin@mzadpalestine.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            FirstName = "Admin",
            LastName = "User",
            IsActive = true,
            IsVerified = true
        };

        if (await userManager.FindByEmailAsync(adminUser.Email) == null)
        {
            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}