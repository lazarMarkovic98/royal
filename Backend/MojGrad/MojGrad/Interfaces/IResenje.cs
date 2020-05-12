using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public interface IResenje
    {
        long dodajResenje(Resenje r);
        bool oceni(long idResenja, int ocena);
        public bool obrisiResenje(long idResenja);
        public IQueryable dajResenja(long idPosta);

        public IQueryable dajResenjaKorisnika(long idKorisnika);

        public bool obrisiResenjaPosta(long idPosta);
        public void notifikacijaResenje(long idPosta);
        public void notifikacijaOcena(long idResenja);
        public double brojResenja();
        public IQueryable resenjaUPoslednjih30Dana();
    }
}
