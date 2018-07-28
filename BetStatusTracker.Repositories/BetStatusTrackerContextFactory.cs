namespace BetStatusTracker.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class BetStatusTrackerContextFactory : IDesignTimeDbContextFactory<BetStatusTrackerContext>
    {
        public BetStatusTrackerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BetStatusTrackerContext>();

            // https://docs.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.infrastructure.sqlserverdbcontextoptionsbuilder
            optionsBuilder.UseSqlServer("Server=localhost;Database=DatabaseName;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=SSPI", x => x.UseRowNumberForPaging());
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=localhost;Database=BetStatusTracker;Trusted_Connection=True;");

            return new BetStatusTrackerContext(optionsBuilder.Options);
        }
    }
}
