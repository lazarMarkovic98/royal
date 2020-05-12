using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Interfaces;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Repo
{
    public class ResenjeRepo : IResenje
    {
        Context db;
        ISlika s;
        public ResenjeRepo(Context db,ISlika s)
        {
            this.db = db;
            this.s = s;
        }

        public double brojResenja()
        {
            return db.Resenja.Count();
        }

        public IQueryable dajResenja(long idPosta)
        {
            try
            {
                var resenja = db.Resenja.Where(x => x.IzazovId == idPosta).OrderByDescending(x => x.Ocena).ThenByDescending(x => x.Datum).Select(x => x);
                var post = db.Izazovi.Find(idPosta);
                if (post == null) return null;
                var res = from r in resenja
                          join u in db.Korisnici on r.KorisnikId equals u.Id
                          select new
                          {
                              autorPosta = post.KorisnikId,
                              id=r.Id,
                              userId=r.KorisnikId,
                              userImg = URL.ProfileUrl + u.Slika,
                              username = u.KorisnickoIme,
                              datum = r.Datum,
                              opis = r.Opis,
                              ocena = r.Ocena,
                              slike = db.SlikeResenja.Where(x => x.ResenjeId == r.Id).Select(x => URL.SolutionUrl + x.Naziv).ToList()
                          };
                return res;
            }
            catch
            {
                return null;
            }
        }

        public bool obrisiResenje(long idResenja)
        {
            try
            {
                var resenje = db.Resenja.Where(x => x.Id == idResenja).FirstOrDefault();
                var user = db.Korisnici.Find(resenje.KorisnikId);
                user.Poeni -= resenje.Ocena;
                if (s.izbrisiSlikeResenja(resenje.Id) == false) return false;
                db.Resenja.Remove(resenje);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public long dodajResenje(Resenje r)
        {
            try
            {
                if (proveriValidnost(r.KorisnikId, r.IzazovId) >0) return -1;
                db.Resenja.Add(r);
                db.SaveChanges();
                
                var res = db.Resenja.Where(x => x.KorisnikId == r.KorisnikId && x.IzazovId == r.IzazovId).FirstOrDefault();
                if (res == null) return -1;
                return res.Id;
            }
            catch
            {
                return -1;
            }
        }

        public void notifikacijaOcena(long idResenja)
        {
            var resenje = db.Resenja.Find(idResenja);
            var user = db.Korisnici.Find(resenje.KorisnikId);
            Notifications.dodatoResenje(user.FcmToken, resenje.IzazovId,(int)tipNotifikacije.ocena);
        }

        public void notifikacijaResenje(long idPosta)
        {
            try
            {
                var post = db.Izazovi.Find(idPosta);
                var userId = post.KorisnikId;
                var fcm = db.Korisnici.Find(userId).FcmToken;
                Notifications.dodatoResenje(fcm, idPosta,(int)tipNotifikacije.resenje);
            }
            catch
            {
                return;
            }
        }

        public bool obrisiResenjaPosta(long idPosta)
        {
            try
            {
                var resenja = db.Resenja.Where(x => x.IzazovId == idPosta).Select(x => x);

                foreach (var r in resenja)
                {
                    var res = obrisiResenje(r.Id);
                    if (res == false) return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool oceni(long idResenja, int ocena)
        {
            try
            {
                var resenje = db.Resenja.Where(x => x.Id == idResenja).FirstOrDefault();
                if (resenje == null) return false;
                resenje.Ocena = ocena;
                var user = db.Korisnici.Where(x => x.Id == resenje.KorisnikId).FirstOrDefault();
                if (user == null) return false;
                user.Poeni += ocena;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable resenjaUPoslednjih30Dana()
        {
            var dani = new List<DateTime>();
            for (var i = 0; i <= 30; i++)
            {
                dani.Add(DateTime.Now.AddDays(i * -1));
            }

           var resenja = db.Resenja.Where(x => x.Datum.Date > DateTime.Now.AddDays(-30)).GroupBy(x => x.Datum.Date, x => x.Id, (key, g) => new { datum = key, broj = g.Count() });

            var res = from d in dani
                      join r in resenja on d.Date equals r.datum.Date into dr
                      from r in dr.DefaultIfEmpty()
                      select new { datum = d, broj = r == null ? 0 : r.broj };

            return res.AsQueryable();

        }

        private int proveriValidnost(long userId, long postId)
        {
            return  db.Resenja.Where(x => x.KorisnikId == userId && x.IzazovId == postId).Count();
        }

        public IQueryable dajResenjaKorisnika(long idKorisnika)
        {
            var res = from r in db.Resenja
                      where r.KorisnikId == idKorisnika
                      join p in db.Izazovi on r.IzazovId equals p.Id
                      join u in db.Korisnici on p.KorisnikId equals u.Id
                      select new
                      {
                          naslov = p.Naslov,
                          opis = p.Opis,
                          userId = u.Id,
                          username = u.KorisnickoIme,
                          postId = p.Id,
                          prvaSlika = URL.PostUrl + db.SlikeIzazova.Where(x => x.IzazovId == p.Id).Select(x => x.Naziv).FirstOrDefault(),
                          slikaResenja = (from sr in db.SlikeResenja
                                          where sr.ResenjeId == r.Id
                                          select URL.SolutionUrl + sr.Naziv).FirstOrDefault(),
                          x = p.X,
                          y = p.Y,
                      };
            return res;
        }
    }
}
