using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class Nadleznost
    {
        public long Id { get; set; }
        public Korisnik Korisnik { get; set; }
        public long KorisnikId { get; set; }
        public Kategorija Kategorija { get; set; }
        public long KategorijaId { get; set; }
    }
}
