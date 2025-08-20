using Microsoft.EntityFrameworkCore;
using Proposta.Domain.Entities;

namespace Proposta.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<PropostaEntity> Propostas { get; set; }
    }
}
