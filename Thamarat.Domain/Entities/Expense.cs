using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
    public class Expense
    {
       
            public int Id { get; set; }
            public string Description { get; set; } = string.Empty;
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }

            public int? CashAccountId { get; set; } // ✅ المفتاح الأجنبي
            public CashAccount? CashAccount { get; set; } // ✅ العلاقة
        

    }
}
