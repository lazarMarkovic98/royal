using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class Ucesnik
    {
        public long Id { get; set; }
        public Korisnik Korisnik { get; set; }
        public long KorisnikId { get; set; }
        public Dogadjaj Dogadjaj { get; set; }
        public long DogadjajId { get; set; }
    }
}
