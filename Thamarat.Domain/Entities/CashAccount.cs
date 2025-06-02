using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
    public class CashAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Revenue>? Revenues { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
        public ICollection<CashTransaction>? CashTransactions { get; set; }
        public ICollection<WorkerSalaryPayment>? SalaryPayments { get; set; }
    }

}
