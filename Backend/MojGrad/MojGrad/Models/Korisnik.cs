using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public enum rola
    {
        Korisnik,
        Institucija,
        Admin
    }
    public class Korisnik
    {
        public long Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }

        public string KorisnickoIme { get; set; }

        public string Lozinka { get; set; }
        public string Slika { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string FcmToken { get; set; }
        public DateTime Ban { get; set; }
        public int Rola { get; set; }
        public int Poeni { get; set; }
       public double Radius { get; set; }
    }
}
