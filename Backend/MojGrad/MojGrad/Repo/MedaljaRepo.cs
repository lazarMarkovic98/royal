using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Interfaces;
using MojGrad.Models;
using MojGrad.Services;
using Quartz;

namespace MojGrad.Repo
{
    

    public class MedaljaRepo : IMedalja,IJob
    {
        private Context db;

        public MedaljaRepo(Context db)
        {
            this.db = db;
        }

        public IQueryable dajMedaljeKorisnika(long id)
        {
            var medalje = from m in db.Medalje
                          join dm in db.DodeljeneMedalje on m.Id equals dm.MedaljaId
                          where dm.KorisnikId == id
                          orderby dm.Datum descending
                          select new
                          {
                              opis = m.Opis,
                              slika = URL.MedalsUrl + m.Slika,
                              datum = dm.Datum
                          };
            return medalje;
        }

        public bool dodeliMedaljuZaKorisnikaMeseca()
        {
            try
            {
                var users = from u in db.Korisnici
                            join r in db.Resenja on u.Id equals r.KorisnikId
                            where r.Datum > DateTime.Now.AddDays(-30)
                            select new
                            {
                                userId = u.Id,
                                resenjeId = r.Id
                            };
                var top = users.GroupBy(x => x.userId, x => x.resenjeId, (key, g) => new { userId = key, broj = g.Count() }).OrderByDescending(x => x.broj);
                var max = top.FirstOrDefault().broj;
                foreach (var i in top)
                {
                    if (i.broj < max) break;
                    var idMedalje = db.Medalje.Where(x => x.Naziv == "Korisnik meseca").FirstOrDefault().Id;
                    DodeljenaMedalja d = new DodeljenaMedalja();
                    d.MedaljaId = idMedalje;
                    d.KorisnikId = i.userId;
                    d.Datum = DateTime.Now;
                    db.DodeljeneMedalje.Add(d);



                }
                db.SaveChangesAsync();
                return true;
            }
            catch{
                return false;
            }
            
        }

        public bool dodeliMedaljuZaPoene(long userId)
        {
            try
            {
                var user = db.Korisnici.Find(userId);
                var medalje = from m in db.Medalje
                              join dm in db.DodeljeneMedalje on m.Id equals dm.MedaljaId
                              where dm.KorisnikId==userId
                              select new
                              {
                                  naziv = m.Naziv
                              };
                if (user.Poeni >= 500)
                {
                    var c = medalje.Where(x => x.naziv == "500").FirstOrDefault();
                    if (c == null)
                    {
                        var idMedalje = db.Medalje.Where(x => x.Naziv == "500").FirstOrDefault().Id;
                        DodeljenaMedalja d = new DodeljenaMedalja();
                        d.MedaljaId = idMedalje;
                        d.KorisnikId = userId;
                        d.Datum = DateTime.Now;
                        db.DodeljeneMedalje.Add(d);
                        db.SaveChangesAsync();
                        return true;
                    }
                }
                if (user.Poeni >= 300)
                {
                    var c = medalje.Where(x => x.naziv == "300").FirstOrDefault();
                    if (c == null)
                    {
                        var idMedalje = db.Medalje.Where(x => x.Naziv == "300").FirstOrDefault().Id;
                        DodeljenaMedalja d = new DodeljenaMedalja();
                        d.MedaljaId = idMedalje;
                        d.KorisnikId = userId;
                        d.Datum = DateTime.Now;
                        db.DodeljeneMedalje.Add(d);
                        db.SaveChangesAsync();
                        return true;
                    }
                }
                if (user.Poeni >= 100)
                {
                    var c = medalje.Where(x => x.naziv == "100").FirstOrDefault();
                    if (c == null)
                    {
                        var idMedalje = db.Medalje.Where(x => x.Naziv == "100").FirstOrDefault().Id;
                        DodeljenaMedalja d = new DodeljenaMedalja();
                        d.MedaljaId = idMedalje;
                        d.KorisnikId = userId;
                        d.Datum = DateTime.Now;
                        db.DodeljeneMedalje.Add(d);
                        db.SaveChangesAsync();
                        return true;
                    }
                }
                dodeliMedaljuZaNajboljegKorisnika(userId,user.Poeni);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            dodeliMedaljuZaKorisnikaMeseca();
            return Task.CompletedTask;
        }

        private bool dodeliMedaljuZaNajboljegKorisnika(long userId, int poeni)
        {
            try
            {
                var user = db.Korisnici.Where(x => x.Poeni > poeni).FirstOrDefault();
                if (user == null)
                {
                    var medalje = from m in db.Medalje
                                  join dm in db.DodeljeneMedalje on m.Id equals dm.MedaljaId
                                  where dm.KorisnikId == userId
                                  select new
                                  {
                                      naziv = m.Naziv
                                  };

                    var c = medalje.Where(x => x.naziv == "Najbolji korisnik").FirstOrDefault();
                    if (c == null)
                    {
                        var idMedalje = db.Medalje.Where(x => x.Naziv == "Najbolji korisnik").FirstOrDefault().Id;
                        DodeljenaMedalja d = new DodeljenaMedalja();
                        d.MedaljaId = idMedalje;
                        d.KorisnikId = userId;
                        d.Datum = DateTime.Now;
                        db.DodeljeneMedalje.Add(d);
                        db.SaveChangesAsync();
                        return true;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }
       

    }
}
