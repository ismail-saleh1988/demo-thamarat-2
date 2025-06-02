using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
    public class WorkerAdvance
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public int CashAccountId { get; set; }
        public CashAccount? CashAccount { get; set; }

        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

}
