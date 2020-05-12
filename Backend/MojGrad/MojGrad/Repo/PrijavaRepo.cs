using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Repo
{
    public class PrijavaRepo : IPrijava
    {
        Context db;
        public PrijavaRepo(Context db)
        {
            this.db=db;
        }

        public double brojPrijava()
        {
            return db.Prijave.Count();
        }

        public List<BrojPrijava> dajPrijave()
        {
            var prijave = db.Prijave.GroupBy(p => p.IzazovId).Select(x => new BrojPrijava { idObjave = x.Key, brojPrijava = x.Count() }).ToList();
            return prijave;
        }

        public List<BrojPrijava> dajPrijaveDogadjaja()
        {
            var prijave = db.PrijaveDogadjaja.GroupBy(p => p.DogadjajId).Select(x => new BrojPrijava { idObjave = x.Key, brojPrijava = x.Count() }).ToList();
            return prijave;
        }

        public bool dodajPrijavu(long idPosta,long idUsera)
        {
            Prijava p = new Prijava();
            p.IzazovId = idPosta;
            p.KorisnikId = idUsera;
            p.Datum = DateTime.Now;
            try
            {
                db.Prijave.Add(p);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool izbrisiDogadjaj(long idDogadjaja)
        {
            try
            {
                var dogadjaj = db.Dogadjaji.Find(idDogadjaja);
                var prijave = db.PrijaveDogadjaja.Where(x => x.DogadjajId == idDogadjaja).Select(x => x).ToList();

                db.PrijaveDogadjaja.RemoveRange(prijave);
                db.Dogadjaji.Remove(dogadjaj);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool izbrisiIzazov(long idPosta)
        {
            try
            {

                var post = db.Izazovi.Where(x => x.Id == idPosta).FirstOrDefault();
                var prijave = db.Prijave.Where(x => x.IzazovId == idPosta).Select(x => x).ToList();

                db.Prijave.RemoveRange(prijave);
                db.Izazovi.Remove(post);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool izbrisiPrijave(long idPosta)
        {
           
                var prijave= db.Prijave.Where(x => x.IzazovId == idPosta).Select(x => x).ToList();
                try
                {
                    db.Prijave.RemoveRange(prijave);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
        }

        public bool izbrisiPrijaveDogadjaja(long idDogadjaja)
        {
            var prijave = db.PrijaveDogadjaja.Where(x => x.DogadjajId == idDogadjaja).Select(x => x).ToList();
            try
            {
                db.PrijaveDogadjaja.RemoveRange(prijave);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool prijaviDogadjaj(PrijavaDogadjaja p)
        {
            try
            {
                db.PrijaveDogadjaja.Add(p);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool prijaviIzazov(Prijava p)
        {
            try
            {
                db.Prijave.Add(p);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class BrojPrijava
    {
        public long idObjave { get; set; }
        public int brojPrijava { get; set; }
    }
}
