using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public interface ISlika
    {
        public string dajPrvuSLiku(long IdPosta);

        public string dodajSliku(long IdPosta, Image img);

        public string dodajSlikuEventa(string img);

        public string promeniSlikuProfila(long UserId, string slika);
        public bool izbrisiSlikePosta(long idPosta);
        public string dodajSlikuResenja(long idResenja, Image img);

        public bool izbrisiSlikeResenja(long idResenja);
        public bool izbrisiSlikuEventa(string naziv);
    }
}
