using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
    public class CashTransaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // In / Out
        public string Description { get; set; }

        public int CashAccountId { get; set; }
        public CashAccount CashAccount { get; set; }
    }

}
