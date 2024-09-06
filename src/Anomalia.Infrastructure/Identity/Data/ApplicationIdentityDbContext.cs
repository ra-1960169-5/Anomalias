using Anomalias.Infrastructure.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Infrastructure.Identity.Data
{
    public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : IdentityDbContext<IdentityUser, Role, string>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {        
            base.OnModelCreating(modelBuilder);
        }

    }
}
