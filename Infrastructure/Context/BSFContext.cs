using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class BSFContext : DbContext
    {
        public BSFContext(DbContextOptions<BSFContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var relationShips = modelBuilder.Model
                .GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in relationShips)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<ServiceCategory>().HasData(
                new ServiceCategory { Id = 1, Name = "Plumbing", Code = Domain.Enums.ServiceCategoryEnum.Plumbing },
                new ServiceCategory { Id = 2, Name = "Electrical", Code = Domain.Enums.ServiceCategoryEnum.Electrical },
                new ServiceCategory { Id = 3, Name = "Cleaning", Code = Domain.Enums.ServiceCategoryEnum.Cleaning },
                new ServiceCategory { Id = 4, Name = "Landscaping", Code = Domain.Enums.ServiceCategoryEnum.Landscaping },
                new ServiceCategory { Id = 5, Name = "Painting", Code = Domain.Enums.ServiceCategoryEnum.Painting },
                new ServiceCategory { Id = 6, Name = "Carpentry", Code = Domain.Enums.ServiceCategoryEnum.Carpentry },
                new ServiceCategory { Id = 7, Name = "HVAC", Code = Domain.Enums.ServiceCategoryEnum.HVAC },
                new ServiceCategory { Id = 8, Name = "Roofing", Code = Domain.Enums.ServiceCategoryEnum.Roofing },
                new ServiceCategory { Id = 9, Name = "PestControl", Code = Domain.Enums.ServiceCategoryEnum.PestControl },
                new ServiceCategory { Id = 10, Name = "MovingServices", Code = Domain.Enums.ServiceCategoryEnum.MovingServices }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<ClientUser> ClientUsers { get; set; }
        public DbSet<RefershToken> RefershTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Chat> chats { get; set; }
         public DbSet<ChatMessage> ChatMessages { get; set; }


    }
}