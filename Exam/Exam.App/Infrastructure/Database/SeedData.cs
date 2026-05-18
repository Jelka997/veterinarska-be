using Exam.App.Domain;
using Microsoft.AspNetCore.Identity;

namespace Exam.App.Infrastructure.Database;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        var admin1 = new ApplicationUser
        {
            UserName = "john",
            Email = "john.doe@example.com",
            Name = "John",
            Surname = "Doe",
            EmailConfirmed = true
        };

        if (await userManager.FindByNameAsync(admin1.UserName) == null)
        {
            await userManager.CreateAsync(admin1, "John123!");
            await userManager.AddToRoleAsync(admin1, "Administrator");
        }

        var admin2 = new ApplicationUser
        {
            UserName = "jane",
            Email = "jane.doe@example.com",
            Name = "Jane",
            Surname = "Doe",
            EmailConfirmed = true
        };

        if (await userManager.FindByNameAsync(admin2.UserName) == null)
        {
            await userManager.CreateAsync(admin2, "Jane123!");
            await userManager.AddToRoleAsync(admin2, "Administrator");
        }

        var owner1 = new ApplicationUser
        {
            UserName = "marko",
            Email = "marko@example.com",
            Name = "Marko",
            Surname = "Markovic",
            EmailConfirmed = true
        };

        if (await userManager.FindByNameAsync(owner1.UserName) == null)
        {
            var result = await userManager.CreateAsync(owner1, "Test123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(owner1, "Owner");
                context.Owners.Add(new Owner { UserId = owner1.Id });
            }
        }

        var owner2 = new ApplicationUser
        {
            UserName = "pera",
            Email = "pera@example.com",
            Name = "Pera",
            Surname = "Peric",
            EmailConfirmed = true
        };

        if (await userManager.FindByNameAsync(owner2.UserName) == null)
        {
            var result = await userManager.CreateAsync(owner2, "Test123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(owner2, "Owner");
                context.Owners.Add(new Owner { UserId = owner2.Id });
            }
        }

        var assist1 = new ApplicationUser
        {
            UserName = "sava",
            Email = "sava@example.com",
            Name = "Sava",
            Surname = "Savic",
            EmailConfirmed = true
        };

        if (await userManager.FindByNameAsync(assist1.UserName) == null)
        {
            var result = await userManager.CreateAsync(assist1, "Test123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(assist1, "Assistant");
                context.Assistants.Add(new Assistant { UserId = assist1.Id });
            }
        }

        var assist2 = new ApplicationUser
        {
            UserName = "marija",
            Email = "marija@example.com",
            Name = "Marija",
            Surname = "Jovanovic",
            EmailConfirmed = true
        };

        if (await userManager.FindByNameAsync(assist2.UserName) == null)
        {
            var result = await userManager.CreateAsync(assist2, "Test123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(assist2, "Assistant");
                context.Assistants.Add(new Assistant { UserId = assist2.Id });
            }
        }

        var vet1 = new ApplicationUser
        {
            UserName = "daca",
            Email = "daca@example.com",
            Name = "Danilo",
            Surname = "Savic",
            EmailConfirmed = true
        };

        if (await userManager.FindByNameAsync(vet1.UserName) == null)
        {
            var result = await userManager.CreateAsync(vet1, "Test123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(vet1, "Vet");
                context.Vets.Add(new Vet { UserId = vet1.Id });
            }
        }

        var vet2 = new ApplicationUser
        {
            UserName = "luka",
            Email = "luka@example.com",
            Name = "Luka",
            Surname = "Jovanovic",
            EmailConfirmed = true
        };

        if (await userManager.FindByNameAsync(vet2.UserName) == null)
        {
            var result = await userManager.CreateAsync(vet2, "Test123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(vet2, "Vet");
                context.Vets.Add(new Vet { UserId = vet2.Id });
            }
        }
        await context.SaveChangesAsync();
    }
}