using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class Dogadjaj
    {
        public long Id { get; set; }
        public DateTime Datum { get; set; }
        public string Naslov { get; set; }
        public string Slika { get; set; }
        public string Opis { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Korisnik Korisnik { get; set; }
        public long KorisnikId { get; set; }
    }
}
