using Microsoft.EntityFrameworkCore;
using PlayTestContainers.Data;
using PlayTestContainers.Data.Model;
using Testcontainers.PostgreSql;

namespace PlayTestContainers.Tests;

public class Tests
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    [OneTimeSetUp]
    public async Task Setup()
    {
        await _postgres.StartAsync();
        var options = new DbContextOptionsBuilder<TestContainersAppContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;
        await using var ctx = new TestContainersAppContext(options);
        await ctx.Database.EnsureCreatedAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _postgres.DisposeAsync();
    }

    [Test]
    public async Task RunDbInTestContainer()
    {
        var options = new DbContextOptionsBuilder<TestContainersAppContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        using (var ctx = new TestContainersAppContext(options))
        {
            ctx.Enities.Add(new SomeEnity
            {
                SomeValue = "Test2"
            });
            await ctx.SaveChangesAsync();
        }

        using (var ctx = new TestContainersAppContext(options))
        {
            Assert.That(await ctx.Enities.CountAsync(), Is.EqualTo(1));
        }
    }
}