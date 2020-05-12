using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Repo
{
    public class DogadjajRepo : IDogadjaj
    {
        Context db;
        ISlika s;
        public DogadjajRepo(Context db,ISlika s)
        {
            this.db = db;
            this.s = s;
        }

        public bool proveriPrisustvo(long idUsera, long idEventa)
        {
            var n = db.Ucesnici.Where(x => x.KorisnikId == idUsera && x.DogadjajId == idEventa).Count();
            if (n == 0) return false;
            else return true;
        }

        public double brojDogadjaja()
        {
            return db.Dogadjaji.Count();
        }

        public object dajDetalje(long Id)
        {
            var ev = db.Dogadjaji.Where(x => x.Id == Id).Select(x => x).FirstOrDefault();
            var org = db.Korisnici.Where(x => x.Id == ev.KorisnikId).Select(x => x).FirstOrDefault();

            var komentari = from k in db.KomentariDogadjaja
                            join e in db.Dogadjaji on k.DogadjajId equals e.Id
                            join u in db.Korisnici on k.KorisnikId equals u.Id
                            orderby k.Datum descending
                            select new
                            {
                                userImg = URL.ProfileUrl + u.Slika,
                                userId = u.Id,
                                username = u.KorisnickoIme,
                                text = k.Text,
                                komentarId = k.Id,
                                ocena = k.Ocena
                            };
            var listaKomentara = komentari.ToList();

            return new
            {
                x = ev.X,
                y = ev.Y,
                id =ev.Id,
                userId = org.Id,
                userImg = URL.ProfileUrl+org.Slika,
                username = org.KorisnickoIme,
                naslov = ev.Naslov,
                opis = ev.Opis,
                slika = URL.EventUrl+ev.Slika,
                datum = ev.Datum,
                komentari=listaKomentara
            };
        }

        public IQueryable dajDogadjaje(double x, double y,double radius)
        {
            var eventi = this.db.Dogadjaji.Select(x => x).AsEnumerable().Where(a => jeUKrugu(x, y, a.X, a.Y, radius));

            var res = from e in eventi
                      orderby e.Datum
                      join u in this.db.Korisnici on e.KorisnikId equals u.Id
                      select new
                      {

                          x=e.X,
                          y=e.Y,
                          idEventa = e.Id,
                          naslov = e.Naslov,
                          idKorisnika = u.Id,
                          username = u.KorisnickoIme,
                          userImg = URL.ProfileUrl+u.Slika,
                          eventImg = URL.EventUrl + e.Slika,
                          datum = e.Datum
                      };
            return res.AsQueryable();
        }

        public IQueryable dajDogadjajeKorisnika(long Id)
        {
            var eventi = this.db.Dogadjaji.Where(x => x.KorisnikId==Id).Select(x => x);

            var res = from e in eventi
                      orderby e.Datum descending
                      join u in this.db.Korisnici on e.KorisnikId equals u.Id
                      select new
                      {
                          idEventa = e.Id,
                          naslov = e.Naslov,
                          idKorisnika = u.Id,
                          username = u.KorisnickoIme,
                          userImg = URL.ProfileUrl + u.Slika,
                          eventImg = URL.EventUrl + e.Slika,
                          datum = e.Datum
                      };
            return res;
        }

        public IQueryable dajZainteresovane(long idEventa)
        {
            try
            {
                var zainteresovani = from u in db.Ucesnici
                                     where u.DogadjajId==idEventa
                                     join x in db.Korisnici on u.KorisnikId equals x.Id
                                     select new
                                     {
                                         username = x.KorisnickoIme,
                                         ime = x.Ime,
                                         prezime = x.Prezime,
                                         id = x.Id,
                                         slika = URL.ProfileUrl + x.Slika
                                     };
                return zainteresovani;
            }
            catch
            {
                return null;
            }
        }

        public bool dodajDogadjaj(Dogadjaj e)
        {
            try
            {
                db.Dogadjaji.Add(e);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public IQueryable dogadjajiIdem(long idUsera)
        {
            var e = db.Ucesnici.Where(x => x.KorisnikId == idUsera);

            var res = from ev in e
                      join ev1 in db.Dogadjaji on ev.DogadjajId equals ev1.Id
                      select new
                      {
                          datum = ev1.Datum,
                          naslov = ev1.Naslov
                      };
            return res;
        }

        public IQueryable dogadjajiUPoslednjih30Dana()
        {
            var dani = new List<DateTime>();
            for(var i = 0; i <= 30; i++)
            {
                dani.Add(DateTime.Now.AddDays(i * -1));
            }

            var eventi = db.Dogadjaji.Where(x => x.Datum.Date >= DateTime.Now.AddDays(-30)).GroupBy(x => x.Datum.Date, x => x.Id, (key, g) => new { datum = key, broj = g.Count() });

            var res = from d in dani
                      join e in eventi on d.Date equals e.datum.Date into de
                      from e in de.DefaultIfEmpty()
                      select new { datum = d, broj = e == null ? 0 : e.broj };

            return res.AsQueryable();
                      
            
                      
        }

        public bool izmeni(long id, string naslov = null, string opis = null, string datum = null, string vreme = null)
        {
            try
            {
                var e = db.Dogadjaji.Find(id);
                if (e == null) return false;
                if (naslov != null) e.Naslov = naslov;
                if (opis != null) e.Opis = opis;
                if (datum != null)
                {
                    var d = DateTime.Parse(datum);
                    var d1 = e.Datum;
                    e.Datum = new DateTime(d.Year, d.Month, d.Day, d1.Hour, d1.Minute, d1.Second);

                }
                if (vreme != null)
                {
                    var d = DateTime.Parse(vreme);
                    var d1 = e.Datum;
                    e.Datum = new DateTime(d1.Year, d1.Month, d1.Day, d.Hour, d.Minute, d.Second);
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool obrisi(long id)
        {
           
            try
            {
                var e = db.Dogadjaji.Find(id);
                if (e == null) return false;
                s.izbrisiSlikuEventa(e.Slika);
                db.Dogadjaji.Remove(e);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<EventSearch> pretraga(string s)
        {
            try
            {
                //var res = this.db.Eventi.Where(x => x.Naslov.ToLower().StartsWith(s.ToLower())).Select(x => x).ToList();
                //return res;

                var res = (from eventt in db.Dogadjaji
                          where eventt.Naslov.ToLower().Contains(s.ToLower())
                          select new EventSearch
                          {
                              id = eventt.Id,
                              naslov = eventt.Naslov,
                              datum = eventt.Datum,
                              slika = URL.EventUrl + eventt.Slika

                          }).Take(10);
                return res.ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool zainteresovan(Ucesnik u)
        {
            try
            {
                var ucesnik = db.Ucesnici.Where(x => x.KorisnikId == u.KorisnikId && x.DogadjajId == u.DogadjajId).FirstOrDefault();
                if (ucesnik == null)
                {
                    db.Ucesnici.Add(u);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    db.Ucesnici.Remove(ucesnik);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
           
        }

        private bool jeUKrugu(double x1, double y1, double x2, double y2, double radius)
        {
            var R = 6371;
            var fi1 = toRadians(x1);
            var fi2 = toRadians(x2);

            var deltaFi = toRadians(x2 - x1);
            var deltaLambda = toRadians(y2 - y1);

            var a = Math.Sin(deltaFi / 2) * Math.Sin(deltaFi / 2) + Math.Cos(fi1) * Math.Cos(fi2) * Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = R * c;

            if (d <= radius) return true;
            return false;
        }
        private double toRadians(double x)
        {
            return (Math.PI / 180) * x;
        }

        public IEnumerable<PrijavljeniDogadjaj> dajPrijavljeneDogadjaje(List<BrojPrijava> prijave)
        {
            var sortiranePrijave = prijave.OrderByDescending(x => x.brojPrijava);

            var res = from prijava in sortiranePrijave
                      join dogadjaj in db.Dogadjaji on prijava.idObjave equals dogadjaj.Id
                      join user in db.Korisnici on dogadjaj.KorisnikId equals user.Id
                      select new PrijavljeniDogadjaj
                      {
                          userId = user.Id,
                          username = user.KorisnickoIme,
                          userImg = URL.ProfileUrl + user.Slika,
                          brojPrijava = prijava.brojPrijava,
                          eventId = dogadjaj.Id,
                          slika = URL.EventUrl+dogadjaj.Slika,
                          razlozi = db.PrijaveDogadjaja.Where(x => x.DogadjajId == dogadjaj.Id).Select(x => x.Opis).ToList()
                      };
            return res;
        }

        public class EventSearch
        {
            public long id { get; set; }
            public string slika { get; set; }
            public string naslov { get; set; }
            public DateTime datum { get; set; }
        }
        public class PrijavljeniDogadjaj
        {
            public long userId { get; set; }
            public string userImg { get; set; }
            public string username { get; set; }
            public long eventId { get; set; }
            public string slika { get; set; }
            public int brojPrijava { get; set; }
            public List<string> razlozi { get; set; }
        }
    }
}
