using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class Izazov
    {
        public long Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }

        public string Naslov { get; set; }
        
        public Kategorija Kategorija { get; set; }
        public long KategorijaId { get; set; }
        public long KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }





    }
}
