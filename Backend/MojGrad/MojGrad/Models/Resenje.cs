using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class Resenje
    {
        public long Id { get; set; }
        public Izazov Izazov { get; set; }
        public long IzazovId { get; set; }
        public Korisnik Korisnik { get; set; }
        public long KorisnikId { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
        public int Ocena { get; set; }
    }
}
