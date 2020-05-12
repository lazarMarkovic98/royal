using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MojGrad.Models
{
    public interface IKomentar
    {
        bool lajkIzazov(int id);

        bool dislajkIzazov(int id);

        bool dodajIzazov(Komentar k);
        bool lajkDogadjaj(int id);

        bool dislajkDogadjaj(int id);

        bool dodajDogadjaj(KomentarDogadjaja k);
        public double brojKomentara();
    }
}
