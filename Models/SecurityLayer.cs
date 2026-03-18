
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventGo.Models;
namespace EventGo.Models
{
    public class SecureContext : IdentityDbContext
    {
        public SecureContext(DbContextOptions<SecureContext>options) : base(options)
        {
            
        }
        public DbSet<MyUser> MyUsers { get; set; }
        public DbSet<MyRole> MyRoles { get; set; }
        public DbSet<EventGo.Models.RegisterViewModel> RegisterViewModel { get; set; } = default!;
        public DbSet<LoginViewModel1> LoginViewModel1 { get; set; } = default!;
        public DbSet<EventGo.Models.RoleViewModel> RoleViewModel { get; set; } = default!;
        public DbSet<EventGo.Models.UserRoleViewModel> UserRoleViewModel { get; set; } = default!;

    }

    public class MyUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
    public class MyRole : IdentityRole
    {
        public  string? desc { get; set; }
    }

}
