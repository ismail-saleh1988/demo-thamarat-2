using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Thamarat.Domain.Entities;

namespace Thamarat.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shareholder> Shareholders { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<CashAccount> CashAccounts { get; set; }
        public DbSet<CashTransaction> CashTransactions { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerCycle> WorkerCycles { get; set; }
        public DbSet<WorkerAdvance> WorkerAdvances { get; set; }
        public DbSet<WorkAttendance> WorkAttendances { get; set; }
        public DbSet<WorkerSalaryPayment> WorkerSalaryPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // علاقات الجداول كما هي
            modelBuilder.Entity<Revenue>()
                .HasOne(r => r.Shareholder)
                .WithMany(s => s.Revenues)
                .HasForeignKey(r => r.ShareholderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Revenue>()
                .HasOne(r => r.CashAccount)
                .WithMany(c => c.Revenues)
                .HasForeignKey(r => r.CashAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.CashAccount)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CashAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CashTransaction>()
                .HasOne(t => t.CashAccount)
                .WithMany(c => c.CashTransactions)
                .HasForeignKey(t => t.CashAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkerCycle>()
                .HasOne(c => c.Worker)
                .WithMany(w => w.WorkerCycles)
                .HasForeignKey(c => c.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkerAdvance>()
                .HasOne(a => a.Worker)
                .WithMany(w => w.Advances)
                .HasForeignKey(a => a.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkAttendance>()
                .HasOne(a => a.Worker)
                .WithMany(w => w.Attendances)
                .HasForeignKey(a => a.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkerSalaryPayment>()
                .HasOne(p => p.Worker)
                .WithMany(w => w.SalaryPayments)
                .HasForeignKey(p => p.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkerSalaryPayment>()
                .HasOne(p => p.CashAccount)
                .WithMany(c => c.SalaryPayments)
                .HasForeignKey(p => p.CashAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
