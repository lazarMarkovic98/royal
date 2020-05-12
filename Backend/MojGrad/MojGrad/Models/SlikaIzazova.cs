using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class SlikaIzazova
    {
        public long Id { get; set; }
        public Izazov Izazov { get; set; }
        public long IzazovId { get; set; }
        public string Naziv { get; set; }
    }
}
