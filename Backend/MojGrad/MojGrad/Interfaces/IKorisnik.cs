using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public interface IKorisnik
    {
        //Ne koristi se
        public IQueryable<Izazov> sveObjaveKorisnika(long Id);

        public Korisnik dajKorisnika(long Id);

        public Korisnik proveriKorisnika(string username, string password);

        public Korisnik registrujKorisnika(string username, string password, string ime, string prezime, string email,string fcm);

        public IQueryable dajSveKorisnike(/*long idPoslednjeg*/);

        public bool promeniKorisnickoIme(long id, string username);
        public bool promeniLozinku(long id, string password,string oldpass=null);
        public bool promeniBio(long id, string bio);
        public bool promeniRadius(long id, double radius);
        public Korisnik nadjiKorisnika(string username, string email);

        public bool osveziFcmToken(string fcm, long id);
        public List<Korisnik> pretraga(string s);
        public double brojKorisnika();
        public bool unapredi(long idUsera);
        public Korisnik dodajInstituciju(string username,string password,string naziv, string email,List<long> kategorije);
        public long proveriAdmina(string token);
        public object prijavaWeb(string user, string pass);
        public bool opomeni(DateTime datum, long userId);
        public DateTime jeOpomenut(long userId);
    }
}
