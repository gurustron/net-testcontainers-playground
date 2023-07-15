using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PlayTestContainers.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestContainersAppContext>
{
    public TestContainersAppContext CreateDbContext(string[] args)
    {
        var cb = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true);
        var config = cb.Build();
        var optionsBuilder = new DbContextOptionsBuilder<TestContainersAppContext>()
            .UseNpgsql(config.GetConnectionString("TestDb"));
        return new TestContainersAppContext(optionsBuilder.Options);
    }
}
