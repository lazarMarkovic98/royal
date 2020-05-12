using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Repo
{
    public class IzazovRepo : IIzazov
    {
        Context db;
        ISlika s;
        IResenje r;
        public IzazovRepo(Context db,ISlika s,IResenje r)
        {
            this.db = db;
            this.s = s;
            this.r = r;
        }

        public double brojIzazova()
        {
            return db.Izazovi.Count();
        }

        public List<Koordinata> dajKoordinate(double x, double y)
        {
            var res = from post in db.Izazovi
                      select new Koordinata
                      {
                          X = post.X,
                          Y = post.Y,
                          Id = post.Id
                      };
            
            
            return res.ToList();
        }

        public Object dajIzazov(long Id)
        {
            var post = db.Izazovi.Where(x => x.Id == Id).Select(x => x);
            var komentari = from k in db.Komentari
                            join p in post on k.PostId equals p.Id
                            join u in db.Korisnici on k.KorisnikId equals u.Id
                            orderby k.Datum descending
                            select new
                            {
                                userImg = URL.ProfileUrl + u.Slika,
                                userId=u.Id,
                                username=u.KorisnickoIme,
                                text=k.Text,
                                komentarId=k.Id,
                                ocena=k.Ocena
                            };

            var listaKomentara = komentari.ToList();
                
                
                var res = from p in post
                      join u in db.Korisnici on p.KorisnikId equals u.Id
                      select new
                      {
                          x=p.X,
                          y=p.Y,
                          naslov = p.Naslov,
                          detalji = p.Opis,
                          userId = u.Id,
                          userImg = URL.ProfileUrl + u.Slika,
                          username = u.KorisnickoIme,
                          datum = p.Datum,
                          kategorija = db.Kategorije.Where(x => x.Id == p.KategorijaId).Select(x => x.Naziv).FirstOrDefault(),
                          slike = db.SlikeIzazova.Where(x => x.IzazovId == p.Id).Select(x => URL.PostUrl + x.Naziv).ToList<string>(),
                          komentari = listaKomentara,
                      };
                      
                
           return res.FirstOrDefault();
        }

        public IQueryable dajIzazoveAdminStrane(long idPoslednjeg)
        {
            IQueryable<Izazov> postovi = null;
            if (idPoslednjeg == 0)
            {
                postovi = db.Izazovi.OrderByDescending(x => x.Datum).Select(x => x).Take(6);
            }
            else
            {
                var lastPost = db.Izazovi.Find(idPoslednjeg);
                postovi = db.Izazovi.Where(x => x.Datum < lastPost.Datum).OrderByDescending(x => x.Datum).Select(x => x).Take(6);
            }

            var res = (from post in postovi
                      orderby post.Datum descending
                      join user in db.Korisnici on post.KorisnikId equals user.Id
                      select new
                      {
                          naslov = post.Naslov,
                          opis = post.Opis,
                          userId = user.Id,
                          //userImg = URL.ProfileUrl + user.Slika,
                          username = user.KorisnickoIme,
                          postId = post.Id,
                          slike = db.SlikeIzazova.Where(x => x.IzazovId == post.Id).Select(x => URL.PostUrl + x.Naziv).ToList(),
                          slikeResenja = (from r in db.Resenja
                                          where r.IzazovId == post.Id
                                          join sr in db.SlikeResenja on r.Id equals sr.ResenjeId
                                          select URL.SolutionUrl + sr.Naziv).ToList(),
                          x = post.X,
                          y = post.Y,
                          datum = post.Datum,
                          brojResenja = db.Resenja.Where(x => x.IzazovId == post.Id).Count(),
                          brojPrijava = db.Prijave.Where(x=>x.IzazovId==post.Id).Count(),
                          brojKomentara = db.Komentari.Where(x=>x.PostId==post.Id).Count(),
                          kategorija = db.Kategorije.Where(x=>x.Id==post.KategorijaId).FirstOrDefault().Naziv
                      }).Take(6);
            return res;
        }

        public IQueryable dajIzazoveKategorije(int id)
        {
            try
            {
                var postovi = db.Izazovi.Where(x => x.KategorijaId == id).Select(x => x);

                var res = from post in postovi
                          orderby post.Datum descending
                          join user in db.Korisnici on post.KorisnikId equals user.Id
                          select new
                          {
                              naslov = post.Naslov,
                              opis = post.Opis,
                              userId = user.Id,
                              //userImg = URL.ProfileUrl + user.Slika,
                              username = user.KorisnickoIme,
                              postId = post.Id,
                              prvaSlika = URL.PostUrl + db.SlikeIzazova.Where(x => x.IzazovId == post.Id).Select(x => x.Naziv).FirstOrDefault()
                          };
                return res;
            }
            catch
            {
                return null;
            }
        }

        public IQueryable dajIzazoveKorisnika(long Id)
        {
            var postovi = db.Izazovi.Where(x => x.KorisnikId == Id).Select(x => x);

            var res = from post in postovi
                      orderby post.Datum descending
                      join user in db.Korisnici on post.KorisnikId equals user.Id
                      select new
                      {
                          naslov=post.Naslov,
                          opis = post.Opis,
                          userId = user.Id,
                          username = user.KorisnickoIme,
                          postId = post.Id,
                          prvaSlika = URL.PostUrl + db.SlikeIzazova.Where(x => x.IzazovId == post.Id).Select(x => x.Naziv).FirstOrDefault(),
                          slikaResenja = (from r in db.Resenja
                                         where r.IzazovId == post.Id
                                         join sr in db.SlikeResenja on r.Id equals sr.ResenjeId
                                         select URL.SolutionUrl+sr.Naziv).FirstOrDefault(),
                          x = post.X,
                          y = post.Y,

                      };
            return res;

        }

        

        public IQueryable dajPostovePocetne(double x, double y)
        {
            var postovi = db.Izazovi/*.Where(a=>jeUKrugu(x,y,a.X,a.Y,radius))*/.Select(x => x);
            var res = from post in postovi
                      orderby post.Datum descending
                      join user in db.Korisnici on post.KorisnikId equals user.Id
                      select new
                      {
                          naslov=post.Naslov,
                          opis=post.Opis,
                          userId = user.Id,
                          //userImg = URL.ProfileUrl + user.Slika,
                          username = user.KorisnickoIme,
                          postId = post.Id,
                          prvaSlika = URL.PostUrl + db.SlikeIzazova.Where(x=>x.IzazovId==post.Id).Select(x => x.Naziv).FirstOrDefault(),
                          slikaResenja = (from r in db.Resenja
                                          where r.IzazovId == post.Id
                                          join sr in db.SlikeResenja on r.Id equals sr.ResenjeId
                                          select URL.SolutionUrl + sr.Naziv).FirstOrDefault(),
                          x = post.X,
                          y = post.Y,
                      };
            return res;

           
        }

        public IEnumerable<PrijavljeniPost> dajPrijavljeneIzazove(List<BrojPrijava> prijave)
        {
            var sortiranePrijave=prijave.OrderByDescending(x => x.brojPrijava);
            
            var res = from prijava in sortiranePrijave
                      join post in db.Izazovi on prijava.idObjave equals post.Id
                      join user in db.Korisnici on post.KorisnikId equals user.Id
                      select new PrijavljeniPost
                      {
                          userId = user.Id,
                          username = user.KorisnickoIme,
                          userImg= URL.ProfileUrl + user.Slika,
                          brojPrijava=prijava.brojPrijava,
                          postId=post.Id,
                          prvaSlika = URL.PostUrl + db.SlikeIzazova.Where(x => x.IzazovId == post.Id).Select(x => x.Naziv).FirstOrDefault(),
                          razlozi = db.Prijave.Where(x=>x.IzazovId==post.Id).Select(x=>x.Text).ToList()
                      };
            return res;
        }

        public bool obrisi(long Id)
        {
            try
            {
                var post = db.Izazovi.Where(x => x.Id == Id).FirstOrDefault();
                if (s.izbrisiSlikePosta(post.Id) == false) return false;
                r.obrisiResenjaPosta(post.Id);
                db.Izazovi.Remove(post);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool izmeni(long idPosta, string naslov, string opis)
        {
            try
            {
                var post = db.Izazovi.Where(x => x.Id == idPosta).FirstOrDefault();
                if (naslov != null)
                {
                    post.Naslov = naslov;
                }
                if (opis != null)
                {
                    post.Opis = opis;
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public long dodaj(Izazov p)
        {
            try
            {
                db.Izazovi.Add(p);
                db.SaveChanges();
                var post=db.Izazovi.Where(x => x.Datum == p.Datum && x.KorisnikId == p.KorisnikId).Select(x => x);
                if (post.Count() != 1) return -1;
                return post.First().Id;
            }
            catch
            {
                return -1;
            }
            
            
        }

      
        public IQueryable ucitajIzazove(double x, double y, long last,double radius)
        {
            IEnumerable<Izazov> postovi = null;
            if (last == 0)
            {

                 postovi = db.Izazovi.OrderByDescending(x => x.Datum).Select(x => x).AsEnumerable().Where(post => jeUKrugu(post.X, post.Y, x, y, radius)).Take(5);
            }
            else
            {
                var lastPost = db.Izazovi.Find(last);
                postovi = db.Izazovi.OrderByDescending(x => x.Datum).Where(x => x.Datum < lastPost.Datum).AsEnumerable().Take(5).Where(a => jeUKrugu(x, y, a.X, a.Y, radius));
            }
            
            var res = from post in postovi
                      join user in db.Korisnici on post.KorisnikId equals user.Id
                      select new
                      {
                          naslov = post.Naslov,
                          userId = user.Id,
                          userImg = URL.ProfileUrl + user.Slika,
                          username = user.KorisnickoIme,
                          postId = post.Id,
                          x=post.X,
                          y=post.Y,
                          prvaSlika = URL.PostUrl + db.SlikeIzazova.Where(x => x.IzazovId == post.Id).Select(x => x.Naziv).FirstOrDefault(),
                          slikaResenja = (from r in db.Resenja
                                          where r.IzazovId == post.Id
                                          join sr in db.SlikeResenja on r.Id equals sr.ResenjeId
                                          select URL.SolutionUrl + sr.Naziv).FirstOrDefault(),
                         
                      };
            return res.AsQueryable();

        }

        public IQueryable loadSolved(double x, double y, long last/*,double radius*/)
        {
            IQueryable<Izazov> postovi = null;
            if (last == 0)
            {
                
                postovi = (from p in db.Izazovi
                           //where jeUKrugu(x,y,p.X,p.Y,radius)
                           orderby p.Datum descending
                           join r in db.Resenja on p.Id equals r.IzazovId
                           select p).Distinct().Take(5);
                          
                         
                         
            }
            else
            {
                var lastPost = db.Izazovi.Find(last);
                
                postovi = (from p in db.Izazovi
                           orderby p.Datum descending
                           where p.Datum < lastPost.Datum //&& jeUKrugu(x, y, p.X, p.Y, radius)
                           join r in db.Resenja on p.Id equals r.IzazovId
                           select p).Distinct().Take(5);
            }
            var res = from post in postovi
                      join user in db.Korisnici on post.KorisnikId equals user.Id
                      select new
                      {
                          naslov = post.Naslov,
                          userId = user.Id,
                          userImg = URL.ProfileUrl + user.Slika,
                          username = user.KorisnickoIme,
                          postId = post.Id,
                          prvaSlika = URL.PostUrl + db.SlikeIzazova.Where(x => x.IzazovId == post.Id).Select(x => x.Naziv).FirstOrDefault(),
                          slikaResenja = (from r in db.Resenja
                                          where r.IzazovId == post.Id
                                          join sr in db.SlikeResenja on r.Id equals sr.ResenjeId
                                          select URL.SolutionUrl + sr.Naziv).FirstOrDefault(),
                          x = post.X,
                          y = post.Y,
                      };
            return res;
        }

        public IQueryable loadUnsolved(double x, double y, long last/*,radius*/)
        {
            IQueryable<Izazov> postovi = null;
            if (last == 0)
            {
                
                postovi = (from p in db.Izazovi
                         //where jeUKrugu(x,y,p.X,p.Y,radius)
                           orderby p.Datum descending
                           join r in db.Resenja on p.Id equals r.IzazovId into pp
                           from r in pp.DefaultIfEmpty()
                           where r==null
                           select p).Take(5);

            }
            else
            {
                var lastPost = db.Izazovi.Find(last);
                postovi = (from p in db.Izazovi
                           orderby p.Datum descending
                           where p.Datum < lastPost.Datum //&& jeUKrugu(x, y, p.X, p.Y, radius)
                           join r in db.Resenja on p.Id equals r.IzazovId into pp
                           from r in pp.DefaultIfEmpty()
                           where r == null
                           select p).Take(5);
            }
            var res = from post in postovi
                      join user in db.Korisnici on post.KorisnikId equals user.Id
                      select new
                      {
                          naslov = post.Naslov,
                          userId = user.Id,
                          userImg = URL.ProfileUrl + user.Slika,
                          username = user.KorisnickoIme,
                          postId = post.Id,
                          prvaSlika = URL.PostUrl + db.SlikeIzazova.Where(x => x.IzazovId == post.Id).Select(x => x.Naziv).FirstOrDefault(),
                          slikaResenja = (from r in db.Resenja
                                          where r.IzazovId == post.Id
                                          join sr in db.SlikeResenja on r.Id equals sr.ResenjeId
                                          select URL.SolutionUrl + sr.Naziv).FirstOrDefault(),
                          x = post.X,
                          y = post.Y,
                      };
            return res;
        }

        public IQueryable izazoviUPoslednjih30Dana()
        {
            var dani = new List<DateTime>();
            for (var i = 0; i <= 30; i++)
            {
                dani.Add(DateTime.Now.AddDays(i * -1));
            }

            var postovi = db.Izazovi.Where(x => x.Datum.Date > DateTime.Now.AddDays(-30)).GroupBy(x => x.Datum.Date, x => x.Id, (key, g) => new { datum = key, broj = g.Count() });

            var res = from d in dani
                      join p in postovi on d.Date equals p.datum.Date into dp
                      from p in dp.DefaultIfEmpty()
                      select new { datum = d, broj = p == null ? 0 : p.broj };

            return res.AsQueryable();
        }

        public List<PostSearch> pretraga(string s)
        {
            try
            {
                var res = (from post in db.Izazovi
                          where post.Naslov.ToLower().Contains(s.ToLower())
                          select new PostSearch
                          {
                              id=post.Id,
                              naslov=post.Naslov,
                              slika= URL.PostUrl + db.SlikeIzazova.Where(x => x.IzazovId == post.Id).Select(x => x.Naziv).FirstOrDefault()
                          }).Take(10);
                return res.ToList();
            }
            catch
            {
                return null;
            }
        }

        public Tuple<double, double> brojResenihINeresenih()
        {
            var ukupno = db.Izazovi.Count();
            var solved = (from p in db.Izazovi
                          join r in db.Resenja on p.Id equals r.IzazovId
                          select p.Id).Distinct().Count();
            return Tuple.Create((double)solved, (double)ukupno - solved);
                         

        }

        public bool jeUKrugu(double x1,double y1,double x2,double y2,double radius)
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
    }

    public class Koordinata
    {
        public long Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class PrijavljeniPost
    {
        public long userId { get; set; }
        public string userImg { get; set; }
        public string username { get; set; }
        public long postId { get; set; }
        public string prvaSlika { get; set; }
        public int brojPrijava { get; set; }
        public List<string> razlozi { get; set; }
    }

    public class PostSearch
    {
        public long id { get; set; }
        public string naslov { get; set; }
        public string slika { get; set; }
    }
}
