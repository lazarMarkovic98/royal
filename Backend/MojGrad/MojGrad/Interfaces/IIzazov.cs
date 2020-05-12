using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Repo;

namespace MojGrad.Models
{
    public interface IIzazov
    {
        public long dodaj(Izazov p);

        public Object dajIzazov(long Id);


        //Ne koristi se
        public IQueryable dajPostovePocetne(double x, double y);

        public List<Koordinata> dajKoordinate(double x, double y);

        public IQueryable dajIzazoveKorisnika(long Id);

        public bool obrisi(long Id);

        public IEnumerable<PrijavljeniPost> dajPrijavljeneIzazove(List<BrojPrijava> prijave);

        public bool izmeni(long idPosta, string naslov, string opis);

        public IQueryable dajIzazoveKategorije(int id);

        public IQueryable ucitajIzazove(double x, double y,long last,double radius);

        public List<PostSearch> pretraga(string s);

        //Ne koristi se
        public IQueryable loadSolved(double x, double y, long last);
        //Ne koristi se
        public IQueryable loadUnsolved(double x, double y, long last);
        public double brojIzazova();
        public Tuple<double,double> brojResenihINeresenih();

        public IQueryable izazoviUPoslednjih30Dana();
        public IQueryable dajIzazoveAdminStrane(long idPoslednjeg);
        

    }
}
