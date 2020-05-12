﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public class PrijavaDogadjaja
    {
        public long Id { get; set; }
        public Dogadjaj Dogadjaj { get; set; }
        public long DogadjajId { get; set; }
        public Korisnik Korisnik { get; set; }
        public long KorisnikId { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
    }
}
