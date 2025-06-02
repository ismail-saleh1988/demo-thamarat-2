using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
    public class WorkerSalaryPayment
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public int CashAccountId { get; set; }
        public CashAccount? CashAccount { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DaysWorked { get; set; }

        public decimal DailyWage { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal TotalAdvance { get; set; }
        public decimal NetPay { get; set; }
        public decimal PaidAmount { get; set; }

        public DateTime PaymentDate { get; set; }
    }

}
