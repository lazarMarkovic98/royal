using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public interface IKategorija
    {
       public List<Kategorija> dajKategorije();

        public IQueryable izazoviPoKategorijama();
    }
}
