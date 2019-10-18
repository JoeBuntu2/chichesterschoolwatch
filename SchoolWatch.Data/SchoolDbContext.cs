using Microsoft.EntityFrameworkCore;
using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions options ) :base(options)
        {
            
        }

     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeEntity>()
                .ToTable("Employees")
                .HasKey(x => new {x.EmployeeId});

            modelBuilder.Entity<EmployeeEntity>().Property(x => x.Credit).HasColumnName("Credits");
 
            modelBuilder.Entity<FiscalYearEntity>()
                .ToTable("FiscalYears")
                .HasKey(x => x.FiscalYearId);

            modelBuilder.Entity<RevenueEntity>()
                .ToTable("Revenues")
                .HasKey(x => x.RevenueId);

            modelBuilder.Entity<DistrictEntity>()
                .ToTable("Districts")
                .HasKey(x => x.DistrictId);

            modelBuilder.Entity<BudgetRevenueEntity>()
                .ToTable("BudgetRevenues")
                .HasKey(x => x.BudgetRevenueId);

            modelBuilder.Entity<BudgetEntity>()
                .ToTable("Budgets")
                .HasKey(x => x.BudgetId);

            modelBuilder.Entity<BudgetEntity>()
                .HasOne(x => x.FiscalYear)
                .WithMany(x => x.Budgets);

            modelBuilder.Entity<TopLevelExpenditureEntity>()
                .ToTable("ExpendituresTopLevel")
                .HasKey(x => x.TopLevelId);

            modelBuilder.Entity<MidLevelExpenditureEntity>()
                .ToTable("ExpendituresMidLevel")
                .HasKey(x => x.MidLevelId);

            modelBuilder.Entity<ExpenditureCodeEntity>()
                .ToTable("ExpenditureCodes")
                .HasKey(x => x.Code);

            modelBuilder.Entity<BudgetExpenditureEntity>()
                .ToTable("Expenditures")
                .HasKey(x => x.ExpenditureId);

            modelBuilder.Entity<TotalEnrollmentEntity>()
                .ToTable("TotalDistrictEnrollments")
                .HasKey(x => x.TotalDistrictEnrollmentId);

        }
    }
}
