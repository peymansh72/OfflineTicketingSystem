
namespace OfflineTicketingSystem.Admin;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Entities;
using Helper;
using Data.Enums;

    public static class SeedAdminUser
    {
        public static async Task AddAdminUser(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    await context.Database.MigrateAsync();

                    if (await context.Users.AnyAsync())
                    {
                        Console.WriteLine("Database already has users. No need to seed.");
                        return;
                    }

                    Console.WriteLine("No users found. Seeding admin user...");

                    var adminUser = new UserEntity
                    {
                        FullName = "Admin User",
                        Email = "admin@bargheto.com", 
                        PasswordHash = PasswordHasher.Hash("PaswordStrong@001!"),
                        Role = Role.Admin
                    };
                    context.Users.Add(adminUser);                
                    await context.SaveChangesAsync();             
                    Console.WriteLine("Admin user seeded successfully.");     
                }
                catch (Exception ex)   
                {
                var logger = services.GetRequiredService<ILogger<Program>>();        
                logger.LogError(ex, "An error occurred during database seeding."); 
                }
            }
        }
    }