using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Repo;
using MojGrad.Services;

namespace MojGrad.Models
{
    public interface IPrijava
    {
        public List<BrojPrijava> dajPrijave();
        public List<BrojPrijava> dajPrijaveDogadjaja();

        public bool izbrisiPrijaveDogadjaja(long idDogadjaja);

        public bool dodajPrijavu(long idPosta,long idUsera);

        public bool izbrisiPrijave(long idPosta);

        public bool izbrisiIzazov(long idPosta);

        public bool izbrisiDogadjaj(long idDogadjaja);

        public bool prijaviIzazov(Prijava p);
        public double brojPrijava();
        public bool prijaviDogadjaj(PrijavaDogadjaja p);


    }
}
