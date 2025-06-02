using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Entities
{
    public class Shareholder
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Revenue>? Revenues { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public double Percentage { get; set; }
    }

}
