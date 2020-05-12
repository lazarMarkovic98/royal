using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class DodeljenaMedalja
    {
        public long Id { get; set; }
        public Medalja Medalja { get; set; }
        public int MedaljaId { get; set; }
        public Korisnik Korisnik { get; set; }
        public long KorisnikId { get; set; }
        public DateTime Datum { get; set; }
    }
}
