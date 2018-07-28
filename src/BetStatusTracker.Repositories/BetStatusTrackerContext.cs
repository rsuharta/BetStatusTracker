namespace BetStatusTracker.Repositories
{
    using System;

    using BetStatusTracker.Entity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BetStatusTrackerContext : DbContext
    {
        public BetStatusTrackerContext(DbContextOptions options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder
        //            .UseLazyLoadingProxies()
        //            .UseSqlServer(@"Server=localhost;Database=BetStatusTracker;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            
            builder.ApplyConfiguration(new ApplicationDatabaseConfig());
        }
        
        public class ApplicationDatabaseConfig : IEntityTypeConfiguration<BetGroupEntity>
        {
            public void Configure(EntityTypeBuilder<BetGroupEntity> builder)
            {
                builder.HasKey(e => e.Id);
                builder.HasIndex(e => new { e.BetClientRef, e.SequenceNo });

                builder.Property(e => e.BetClientRef).IsRequired().HasMaxLength(50).IsUnicode(false);
                builder.Property(e => e.SequenceNo).IsRequired().HasMaxLength(10).IsUnicode(false);
                builder.Property(e => e.BetCombinationNo).IsRequired();
                builder.Property(e => e.BetSubmittedTime).IsRequired();
                builder.Property(e => e.StakeAmount).IsRequired();
                builder.Property(e => e.UpdatedDate).IsRequired();
                builder.Property(e => e.CustomerId).IsRequired().HasMaxLength(20).IsUnicode(false);


                // Make the default table name of AspNetUsers to Users
                builder.ToTable("BetGroup");
            }
        }

        //public class ApplicationDatabaseConfig : IEntityTypeConfiguration<BetEntity>
        //{
        //    public void Configure(EntityTypeBuilder<BetEntity> builder)
        //    {
        //        builder.HasKey(e => e.Id);

        //        builder.Property(e => e.ProductType).IsRequired();
        //        builder.Property(e => e.BetType).IsRequired();

        //        builder.Property(e => e.Selections).IsRequired();
        //        builder.Property(e => e.SelectionOutcomes).IsRequired();
        //        builder.Property(e => e.Status).IsRequired();
                
        //        // Make the default table name of AspNetUsers to Users
        //        builder.ToTable("Bet");
        //    }
        //}

        //public class ApplicationDatabaseConfig : IEntityTypeConfiguration<SelectionEntity>
        //{
        //    public void Configure(EntityTypeBuilder<SelectionEntity> builder)
        //    {
        //    }
        //}

        public virtual DbSet<BetGroupEntity> BetGroups { get; set; }
    }
}
