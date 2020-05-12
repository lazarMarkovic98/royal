using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Repo
{
    public class KorisnikRepo : IKorisnik
    {
        Context db;
        MojToken t;
        public KorisnikRepo(Context db)
        {
            this.db = db;
            this.t = new MojToken();
        }

        public double brojKorisnika()
        {
            return db.Korisnici.Count();
        }

        public Korisnik dajKorisnika(long Id)
        {
            var korisnik = db.Korisnici.Find(Id);
            return korisnik;
        }

        public IQueryable dajSveKorisnike(/*long idPoslednjeg*/)
        {
            var users = from user in db.Korisnici
                        select new
                        {
                            id = user.Id,
                            ime = user.Ime,
                            prezime = user.Prezime,
                            slika = URL.ProfileUrl + user.Slika,
                            username = user.KorisnickoIme,
                            Email = user.Email,
                            Bio = user.Bio,
                            brojPoena = user.Poeni,
                            ban = user.Ban,
                            brojPrijava = (from prijava in db.Prijave
                                           join post in db.Izazovi on prijava.IzazovId equals post.Id
                                           where post.KorisnikId == user.Id
                                           select post.Id).Count(),
                            brojPostova = (from post in db.Izazovi
                                           where post.KorisnikId == user.Id
                                           select post.Id).Count(),
                            brojDogadjaja = (from dogadjaj in db.Dogadjaji
                                             where dogadjaj.KorisnikId == user.Id
                                             select dogadjaj.Id).Count(),
                            brojKomentara = (from k in db.Komentari
                                             where k.KorisnikId == user.Id
                                             select k.Id).Count()+
                                             (from k in db.KomentariDogadjaja
                                              where k.KorisnikId == user.Id
                                              select k.Id).Count(),
                            brojResenja = (from r in db.Resenja
                                           where r.KorisnikId==user.Id
                                           select r.Id).Count(),
                            rola=dajRolu(user.Rola),


                        };
            return users;
            
        }
        private static string dajRolu(int r)
        {
            if (r == (int)rola.Institucija) return "Institucija";
            if (r == (int)rola.Admin) return "Administrator";
            else return "Korisnik";

        }
        public Korisnik dodajInstituciju(string username, string password, string naziv, string email,List<long> kategorije)
        {
            try
            {
                var u = db.Korisnici.Where(x => x.KorisnickoIme == username || x.Email == email).Select(x => x);
                if (u.Count() != 0) return null;
                var user = new Korisnik();
                user.KorisnickoIme = username;
                user.Ime = naziv;
                user.Prezime = "";
                user.FcmToken = null;
                user.Email = email;
                user.Bio = naziv;
                user.Ban = default(DateTime);
                user.Slika = "avatar.png";
                user.Poeni = 0;
                user.Rola = (int)rola.Institucija;
                user.Radius = 50.0;


                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                UTF8Encoding utf8 = new UTF8Encoding();

                byte[] data = md5.ComputeHash(utf8.GetBytes(password));
                string passHash = Convert.ToBase64String(data);

                user.Lozinka = passHash;

                db.Korisnici.Add(user);
                db.SaveChanges();
                 foreach(var i in kategorije)
                 {
                     var nadleznost = new Nadleznost();
                     nadleznost.KategorijaId = i;
                     nadleznost.KorisnikId = user.Id;
                     db.Nadleznost.Add(nadleznost);
                 }
                 db.SaveChanges();
                return user;
            }
            catch
            {
                return null;
            }
           
        }

        public DateTime jeOpomenut(long userId)
        {
            var user = db.Korisnici.Find(userId);
            if (user.Ban > DateTime.Now) return user.Ban;
            return default(DateTime);
        }

        public object prijavaWeb(string user, string pass)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] data = md5.ComputeHash(utf8.GetBytes(pass));
            string passHash = Convert.ToBase64String(data);

            try
            {
                var u = db.Korisnici.Where(x => x.KorisnickoIme == user && x.Lozinka == passHash).FirstOrDefault();
                if (u == null) return null;
                if (u.Rola == (int)rola.Admin)
                {
                    return new
                    {
                        username = u.KorisnickoIme,
                        id = u.Id,
                        imePrezime = u.Ime + " " + u.Prezime,
                        img = URL.ProfileUrl + u.Slika,
                        token = t.dajToken(u.Id),
                        radius = u.Radius,
                        rola = "admin"
                    };
                }
                if (u.Rola == (int)rola.Institucija)
                {
                    return new
                    {
                        username = u.KorisnickoIme,
                        id = u.Id,
                        imePrezime = u.Ime + " " + u.Prezime,
                        img = URL.ProfileUrl + u.Slika,
                        token = t.dajToken(u.Id),
                        radius = u.Radius,
                        rola = "institucija"
                    };
                }
                return null;
            }
            catch
            {
                return null;
            }

        }

        public Korisnik nadjiKorisnika(string username, string email)
        {
            try
            {
                if (username != null)
                {
                    var user = db.Korisnici.Where(x => x.KorisnickoIme == username).Select(x => x).FirstOrDefault();
                    return user;
                }
                if (email != null)
                {
                    var user = db.Korisnici.Where(x => x.Email == email).Select(x => x).FirstOrDefault();
                    return user;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool opomeni(DateTime datum, long userId)
        {
            try
            {
                var user = db.Korisnici.Find(userId);
                if (user.Rola == (int)rola.Admin || user.Rola == (int)rola.Institucija) return false;
                user.Ban = datum;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        public bool promeniBio(long id, string bio)
        {
            try
            {
                var user = db.Korisnici.Find(id);
                if (user == null) return false;
                user.Bio = bio;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool promeniLozinku(long id, string password,string oldpassword)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] data = md5.ComputeHash(utf8.GetBytes(password));
            string passHash = Convert.ToBase64String(data);
            byte[] dataold;
            string oldHash="";
            if (oldpassword != null)
            {
                dataold = md5.ComputeHash(utf8.GetBytes(oldpassword));
                oldHash = Convert.ToBase64String(dataold);
            }
       
            try
            {
                var user = db.Korisnici.Find(id);
                if (oldpassword!=null && user.Lozinka != oldHash) return false;
                user.Lozinka = passHash;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool promeniRadius(long id, double radius)
        {
            var user = db.Korisnici.Find(id);
            try
            {
                user.Radius = radius;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool promeniKorisnickoIme(long id, string username)
        {
            var postoji= db.Korisnici.Where(x => x.KorisnickoIme == username).Select(x => x);
            if (postoji.Count() != 0) return false;
            var user = db.Korisnici.Find(id);
            try
            {
                user.KorisnickoIme = username;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
            
        }

        public long proveriAdmina(string token)
        {
            try
            {
                var userId = t.verifikujToken(token);
                if (userId == -1) return -1;
                if (db.Korisnici.Find(userId).Rola != (int)rola.Admin) return -1;
                return userId;
            }
            catch
            {
                return -1;
            }
        }

        public Korisnik proveriKorisnika(string username, string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] data = md5.ComputeHash(utf8.GetBytes(password));
            string passHash = Convert.ToBase64String(data);

            var u = db.Korisnici.Select(x => x).Where(x => x.KorisnickoIme == username && x.Lozinka == passHash).FirstOrDefault();

            if (u!= null && u.Rola!=(int)rola.Institucija)
            {
                return u;
            }
            return null;

        }

        public Korisnik registrujKorisnika(string username, string password, string ime, string prezime, string email,string fcm)
        {
            var u = db.Korisnici.Where(x => x.KorisnickoIme == username || x.Email == email).Select(x => x);
            if (u.Count() != 0) return null;
            Korisnik korisnik = new Korisnik();
            korisnik.Ime = ime;
            korisnik.Prezime = prezime;
            korisnik.KorisnickoIme = username;
            korisnik.Bio = ime + " " + prezime;
            korisnik.FcmToken = fcm;
            korisnik.Rola = (int)rola.Korisnik;
            korisnik.Ban = default(DateTime);
            korisnik.Radius = 10.0;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] data = md5.ComputeHash(utf8.GetBytes(password));
            string passHash = Convert.ToBase64String(data);

            korisnik.Lozinka = passHash;
            korisnik.Slika = "avatar.png";
            korisnik.Email = email;
            korisnik.Poeni = 0;
            db.Korisnici.Add(korisnik);
            db.SaveChanges();

            return korisnik;
        }

        public List<Korisnik> pretraga(string s)
        {
            try
            {
                var u = db.Korisnici.Where(x => x.KorisnickoIme.ToLower().StartsWith(s.ToLower()) ||
                                  (x.Ime+" "+x.Prezime).ToLower().StartsWith(s.ToLower())).Select(x => x).Take(10);
                return u.ToList();
            }
            catch
            {
                return null;
            }
        }

        
        public IQueryable<Izazov> sveObjaveKorisnika(long Id)
        {
            return db.Izazovi.Select(x => x).Where(x => x.KorisnikId == Id);
        }

        public bool unapredi(long idUsera)
        {
            try
            {
                var user = db.Korisnici.Find(idUsera);
                user.Rola = (int)rola.Admin;
                db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool osveziFcmToken(string fcm, long id)
        {
            try
            {
                var user = db.Korisnici.Find(id);
                user.FcmToken = fcm;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
