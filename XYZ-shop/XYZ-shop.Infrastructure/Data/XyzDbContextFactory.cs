
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace XYZ_shop.Infrastructure.Data
{
    public class XyzDbContextFactory : IDesignTimeDbContextFactory<XyzDbContext>
    {
        public XyzDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var solutionPath = Directory.GetParent(basePath)?.Parent?.Parent?.FullName;
            if (solutionPath != null)
            {
                var webProjectPath = Path.Combine(solutionPath, "XYZ-shop.Web");
                if (Directory.Exists(webProjectPath))
                {
                    basePath = webProjectPath;
                }
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<XyzDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultDbConnection"));

            return new XyzDbContext(optionsBuilder.Options);
        }

    }
}
