using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Infrastructure.Context
{
    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlServer("Data Source=DESKTOP-XXX\\SQLEXPRESS01;Initial Catalog=catalog;Persist Security Info=True;User ID=sa;Password=12345;TrustServerCertificate=True");

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}
