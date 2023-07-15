// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayTestContainers.Data;
using PlayTestContainers.Data.Model;

Console.WriteLine("Hello, World!");
var cb = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true);
var config = cb.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<TestContainersAppContext>(builder =>
    builder.UseNpgsql(config.GetConnectionString("TestDb"))
        .LogTo(Console.WriteLine, LogLevel.Information));
        
var serviceProvider = serviceCollection.BuildServiceProvider();
using (var scope = serviceProvider.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<TestContainersAppContext>();
    // ctx.Database.EnsureDeleted();
    // ctx.Database.EnsureCreated();
    ctx.Enities.Add(new SomeEnity
    {
        SomeValue = "Test2"
    });
    await ctx.SaveChangesAsync();
}

using (var scope = serviceProvider.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<TestContainersAppContext>();
    var someEnities = ctx.Enities.ToList();
    Console.WriteLine(someEnities.Count);
}