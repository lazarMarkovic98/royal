using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Repo
{
    public class KomentarRepo : IKomentar
    {
        Context db;

        public KomentarRepo(Context db)
        {
            this.db = db;
        }

        public double brojKomentara()
        {
            return db.Komentari.Count();
        }

        public bool dislajkDogadjaj(int id)
        {
            try
            {
                var k = db.KomentariDogadjaja.Find(id);
                k.Ocena--;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool dislajkIzazov(int id)
        {
            try
            {
                var k = db.Komentari.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
                k.Ocena--;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool dodajDogadjaj(KomentarDogadjaja k)
        {
            try
            {
                k.Ocena = 0;
                k.Datum = DateTime.Now;
                db.KomentariDogadjaja.Add(k);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool dodajIzazov(Komentar k)
        {
            try
            {
                k.Ocena = 0;
                k.Datum = DateTime.Now;
                db.Komentari.Add(k);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool lajkDogadjaj(int id)
        {
            try
            {
                var k = db.KomentariDogadjaja.Find(id);
                k.Ocena++;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool lajkIzazov(int id)
        {
            try
            {
                var k = db.Komentari.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
                k.Ocena++;
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
