using Auth.Core.Models;
using Auth.Core.Security.Hashing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public DatabaseSeeder(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync()
        {
            // Ensure the database is created
            await _context.Database.EnsureCreatedAsync();

            if (await _context.Roles.AnyAsync())
            {
                return;
            }

            var roles = new List<Role> 
            {
                new Role{ Name = ApplicationRole.User.ToString() },
                new Role { Name = ApplicationRole.Administrator.ToString() }
            };

            await _context.Roles.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            if (_context.Users.Any())
            {
                return;
            }

            var users = new List<User>
            {
                new User { Email = "admin@admin.com", PasswordHash = _passwordHasher.HashPassword(null, "12345678") },
                new User { Email = "user@user.com", PasswordHash = _passwordHasher.HashPassword(null, "12345678") }
            };

            users[0].UserRoles.Add(new UserRole
            {
                RoleId = _context.Roles.SingleOrDefault(r => r.Name == ApplicationRole.Administrator.ToString()).Id
            });

            users[1].UserRoles.Add(new UserRole
            {
                RoleId = _context.Roles.SingleOrDefault(r => r.Name == ApplicationRole.User.ToString()).Id
            });

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }
    }
}
