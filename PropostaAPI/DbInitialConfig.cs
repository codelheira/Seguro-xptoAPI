using Proposta.Infrastructure.Data;

namespace PropostaAPI
{
    public static class DbInitialConfig
    {
        public static void ConfigureSQLite(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }
        }
    }
}
