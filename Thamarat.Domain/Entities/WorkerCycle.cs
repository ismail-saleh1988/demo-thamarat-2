using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
    public class WorkerCycle
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }
        public DateTime StartDate { get; set; }
    }

}
