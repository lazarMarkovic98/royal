using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Repo;
using static MojGrad.Repo.DogadjajRepo;

namespace MojGrad.Models
{
    public interface IDogadjaj
    {
        public bool dodajDogadjaj(Dogadjaj e);

        public IQueryable dajDogadjaje(double x, double y,double radius);
        public IEnumerable<PrijavljeniDogadjaj> dajPrijavljeneDogadjaje(List<BrojPrijava> prijave);

        public Object dajDetalje(long Id);

        public List<EventSearch> pretraga(string s);
        public bool zainteresovan(Ucesnik u);
        public IQueryable dajZainteresovane(long idEventa);
        public IQueryable dajDogadjajeKorisnika(long Id);
        public bool proveriPrisustvo(long idUsera, long idEventa);

        public IQueryable dogadjajiIdem(long idUsera);
        public double brojDogadjaja();
        public IQueryable dogadjajiUPoslednjih30Dana();
        public bool izmeni(long id, string naslov = null, string opis = null, string datum = null, string vreme = null);
        public bool obrisi(long id);
    }
}
