using HelpDeskManager.Core.Entities;
using HelpDeskManager.DAL.Data.Identity;
using HelpDeskManager.DAL.Data.Seeding.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


namespace HelpDeskManager.DAL.Data.Seeding;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        await context.Database.MigrateAsync();

        if (await context.Customers.AnyAsync()) return;

        var jsonData = await File.ReadAllTextAsync("Data/seedData.json");
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
        var seedData = JsonSerializer.Deserialize<SeedDataDto>(jsonData, options);

        if (seedData == null) return;

        // Usuarios
        foreach (var userDto in seedData.Users)
        {
            if (await userManager.FindByEmailAsync(userDto.Email) == null)
            {
                var newUser = new AppUser
                {
                    Id = userDto.Id,
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    DocumentType = userDto.DocumentType,
                    DocumentNumber = userDto.DocumentNumber,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, userDto.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, userDto.Role);
                }
            }
        }

        // Clientes
        var customers = seedData.Customers.Select(c => new Customer
        {
            Id = c.Id,
            DocumentType = c.DocumentType,
            DocumentNumber = c.DocumentNumber,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber
        }).ToList();

        await context.Customers.AddRangeAsync(customers);

        // Solicitudes de soporte y comentarios
        var requests = seedData.SupportRequests.Select(r => new SupportRequest
        {
            Id = r.Id,
            CustomerId = r.CustomerId,
            Type = r.Type,
            Subject = r.Subject,
            Description = r.Description,
            Status = r.Status,
            Comments = r.Comments.Select(c => new Comment
            {
                Id = c.Id,
                UserId = c.UserId,
                Message = c.Message
            }).ToList()
        }).ToList();

        await context.SupportRequests.AddRangeAsync(requests);

        // Historial de solicitudes
        var requestHistories = seedData.RequestHistories.Select(h => new RequestHistory
        {
            Id = h.Id,
            SupportRequestId = h.SupportRequestId,
            PreviousStatus = h.PreviousStatus,
            NewStatus = h.NewStatus,
            ChangeNotes = h.ChangeNotes,
            ModifiedByUserId = h.ModifiedByUserId
        }).ToList();

        await context.RequestHistories.AddRangeAsync(requestHistories);

        await context.SaveChangesAsync();
    }
}
