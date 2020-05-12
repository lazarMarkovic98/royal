using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class SlikaResenja
    {
        public long Id { get; set; }
        public Resenje Resenje { get; set; }
        public long ResenjeId { get; set; }
        public string Naziv { get; set; }
    }
}
