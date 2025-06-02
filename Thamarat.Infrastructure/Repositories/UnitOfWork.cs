using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;
using Thamarat.Infrastructure.Persistence;

namespace Thamarat.Infrastructure.Repositories
{
   

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<Expense> Expenses { get; private set; }
        public IRepository<Revenue> Revenues { get; private set; }
        public IRepository<Shareholder> Shareholders { get; private set; }
        public IRepository<CashAccount> CashAccounts { get; private set; }
        public IRepository<Worker> Workers { get; private set; }
        public IRepository<WorkerCycle> WorkerCycles { get; private set; }
        public IRepository<WorkerAdvance> WorkerAdvances { get; private set; }
        public IRepository<WorkerSalaryPayment> WorkerSalaryPayments { get; }
        public IRepository<CashTransaction> CashTransactions { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Expenses = new GenericRepository<Expense>(_context);
            Revenues = new GenericRepository<Revenue>(_context);
            Shareholders = new GenericRepository<Shareholder>(_context);
            CashAccounts = new GenericRepository<CashAccount>(_context);
            Workers = new GenericRepository<Worker>(_context);
            WorkerCycles = new GenericRepository<WorkerCycle>(_context);
            WorkerSalaryPayments = new GenericRepository<WorkerSalaryPayment>(_context);
            CashTransactions = new GenericRepository<CashTransaction>(_context);

            WorkerAdvances = new GenericRepository<WorkerAdvance>(_context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }

}
