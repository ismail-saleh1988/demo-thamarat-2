using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
   

    public class Revenue
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int? ShareholderId { get; set; }
        public Shareholder? Shareholder { get; set; }

        public int? CashAccountId { get; set; }
        public CashAccount? CashAccount { get; set; }
    }

}
