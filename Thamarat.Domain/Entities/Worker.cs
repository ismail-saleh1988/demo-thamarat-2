using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DailyWage { get; set; }
        public DateTime StartDate { get; set; }

        public ICollection<WorkerCycle>? WorkerCycles { get; set; }
        public ICollection<WorkerAdvance>? Advances { get; set; }
        public ICollection<WorkAttendance>? Attendances { get; set; }
        public ICollection<WorkerSalaryPayment>? SalaryPayments { get; set; }
    }

}
