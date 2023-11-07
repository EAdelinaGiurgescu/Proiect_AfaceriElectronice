using Imbracaminte.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Imbracaminte
{
    public class ImbracaminteContext : DbContext
    {
        public ImbracaminteContext(DbContextOptions<ImbracaminteContext> options)
            : base(options) 
        {

        }

        public DbSet<Articole> Articole { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}

