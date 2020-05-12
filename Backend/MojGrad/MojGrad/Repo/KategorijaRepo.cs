using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Repo
{
    public class KategorijaRepo : IKategorija
    {
        Context db;
        public KategorijaRepo(Context db)
        {
            this.db = db;
        }

        public IQueryable izazoviPoKategorijama()
        {
           

           var kategorije = db.Izazovi.GroupBy(x => x.KategorijaId, x => x.Id, (key, g) => new { kategorijaId = key, broj = g.Count() });
            var res = from k in kategorije
                      join k1 in db.Kategorije on k.kategorijaId equals k1.Id
                      select new
                      {
                          naziv = k1.Naziv,
                          broj = k.broj
                      };
            return res;

        }

        public List<Kategorija> dajKategorije()
        {
            return db.Kategorije.ToList();
        }
    }
}
