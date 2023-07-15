using Microsoft.EntityFrameworkCore;
using PlayTestContainers.Data.Model;

namespace PlayTestContainers.Data;

public class TestContainersAppContext : DbContext
{
    public TestContainersAppContext(DbContextOptions<TestContainersAppContext> opts) : base(opts)
    {
    }

    public DbSet<SomeEnity> Enities => Set<SomeEnity>();
}
