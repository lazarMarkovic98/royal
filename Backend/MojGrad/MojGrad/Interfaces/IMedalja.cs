using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Interfaces
{
    public interface IMedalja
    {
        public bool dodeliMedaljuZaPoene(long userId);
        public bool dodeliMedaljuZaKorisnikaMeseca();

        public IQueryable dajMedaljeKorisnika(long id);
    }
}
