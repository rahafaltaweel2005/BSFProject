using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context; 
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Data{
    public static class UserSeedData
    {
        private readonly static string adminPassword = "Admin@123";
        public static void UserSeed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BSFContext>();

            if (!context.Roles.Any())
            {
               var roles = new List<Role>
                {
                    new Role { Name = SystemRole.Admin.ToString(), Code = SystemRole.Admin },
                    new Role { Name = SystemRole.ServiceProvider.ToString(), Code = SystemRole.ServiceProvider },
                    new Role { Name = SystemRole.User.ToString(), Code = SystemRole.User }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var adminRoleId = context.Roles.FirstOrDefault(r => r.Code == SystemRole.Admin).Id;
                var user = new User
                {
                    Name = "Admin User",
                    Email = "admin@bsf.com",
                    PhoneNumber = "00962795213723",
                    RoleId = adminRoleId
                };

                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, adminPassword);

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}