using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thamarat.Domain.Entities;
using Thamarat.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Expense> Expenses { get; }
    IRepository<Revenue> Revenues { get; }
    IRepository<Shareholder> Shareholders { get; }
    IRepository<CashAccount> CashAccounts { get; }
    IRepository<Worker> Workers { get; }
    IRepository<WorkerCycle> WorkerCycles { get; }
    IRepository<WorkerAdvance> WorkerAdvances { get; }

    IRepository<WorkerSalaryPayment> WorkerSalaryPayments { get; }
    IRepository<CashTransaction> CashTransactions { get; }

    Task<int> SaveAsync();
}

