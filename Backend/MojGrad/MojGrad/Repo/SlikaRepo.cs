using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MojGrad.Models;
using MojGrad.Services;
using NPOI.Util;
using System.IO.MemoryMappedFiles;

namespace MojGrad.Repo
{
    public class SlikaRepo : ISlika
    {
        private Context db;
        private IWebHostEnvironment env;
        public SlikaRepo(Context db,IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public string dajPrvuSLiku(long IdPosta)
        {
            var slike = db.SlikeIzazova.Select(x => x).Where(x => x.IzazovId == IdPosta);
            if (slike.Count() == 0) return null;
            else return slike.First().Naziv;
        }

        public string dodajSliku(long IdPosta, Image slika)
        {
            try
            {
                string ext = "";
                
                if (ImageFormat.Jpeg.Equals(slika.RawFormat))
                {
                    ext = "jpg";
                }

                if (ImageFormat.Png.Equals(slika.RawFormat))
                {
                    ext = "png";
                }
                if (ext == "") return "Nepoznata extenzija";
                
                var naziv = string.Format(@"{0}", Guid.NewGuid());

                string folder = Path.Combine(env.WebRootPath, "Images","Post");
                string nazivFajla = naziv + "." + ext;
                string filepath = Path.Combine(folder, nazivFajla);
                slika.Save(filepath);

                SlikaIzazova s = new SlikaIzazova();
                s.Naziv = nazivFajla;
                s.IzazovId = IdPosta;
                
                this.db.SlikeIzazova.Add(s);
                db.SaveChanges();
                return null;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public string dodajSlikuEventa(string img)
        {
            try
            {
                byte[] bajtoviSlike = Convert.FromBase64String(img);



                var stream = new MemoryStream(bajtoviSlike, 0, bajtoviSlike.Length);
                Image slika = Image.FromStream(stream, true);

                string ext = "";

                if (ImageFormat.Jpeg.Equals(slika.RawFormat))
                {
                    ext = "jpg";
                }

                if (ImageFormat.Png.Equals(slika.RawFormat))
                {
                    ext = "png";
                }
                if (ext == "") return null;

                var naziv = string.Format(@"{0}", Guid.NewGuid());

                string folder = Path.Combine(env.WebRootPath, "Images", "Event");
                string nazivFajla = naziv + "." + ext;
                string filepath = Path.Combine(folder, nazivFajla);
                slika.Save(filepath);
                return nazivFajla;
            }
            catch
            {
                return null;
            }
            
        }

        public string dodajSlikuResenja(long idResenja, Image slika)
        {
            try
            {
                string ext = "";

                if (ImageFormat.Jpeg.Equals(slika.RawFormat))
                {
                    ext = "jpg";
                }

                if (ImageFormat.Png.Equals(slika.RawFormat))
                {
                    ext = "png";
                }
                if (ext == "") return "Nepoznata extenzija";

                var naziv = string.Format(@"{0}", Guid.NewGuid());

                string folder = Path.Combine(env.WebRootPath, "Images", "Solution");
                string nazivFajla = naziv + "." + ext;
                string filepath = Path.Combine(folder, nazivFajla);
                slika.Save(filepath);
                
                SlikaResenja s = new SlikaResenja();
                s.Naziv = nazivFajla;
                s.ResenjeId = idResenja;
                this.db.SlikeResenja.Add(s);
                db.SaveChanges();
                return null;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public bool izbrisiSlikePosta(long idPosta)
        {
            try
            {
                var slike = db.SlikeIzazova.Where(x => x.IzazovId == idPosta).Select(x => x).ToList();
                string folder = Path.Combine(env.WebRootPath, "Images", "Post");

                foreach (var slika in slike)
                {
                    string nazivFajla = slika.Naziv;
                    string filepath = Path.Combine(folder, nazivFajla);
                    System.IO.File.Delete(filepath);
                }
                db.SlikeIzazova.RemoveRange(slike);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool izbrisiSlikeResenja(long idResenja)
        {
            try
            {
                var slike = db.SlikeResenja.Where(x => x.ResenjeId == idResenja).Select(x => x).ToList();
                string folder = Path.Combine(env.WebRootPath, "Images", "Solution");

                foreach (var slika in slike)
                {
                    string nazivFajla = slika.Naziv;
                    string filepath = Path.Combine(folder, nazivFajla);
                    System.IO.File.Delete(filepath);
                }
                db.SlikeResenja.RemoveRange(slike);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool izbrisiSlikuEventa(string naziv)
        {
            try
            {
                string path = Path.Combine(env.WebRootPath, "Images", "Event", naziv);
                System.IO.File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string promeniSlikuProfila(long UserId, string img)
        {
            try
            {
                var user = db.Korisnici.Find(UserId);
                var staraSlika = user.Slika;



                byte[] bajtoviSlike = Convert.FromBase64String(img);



                var stream = new MemoryStream(bajtoviSlike, 0, bajtoviSlike.Length);
                Image slika = Image.FromStream(stream, true);

                string ext = "";

                if (ImageFormat.Jpeg.Equals(slika.RawFormat))
                {
                    ext = "jpg";
                }

                if (ImageFormat.Png.Equals(slika.RawFormat))
                {
                    ext = "png";
                }
                if (ext == "") return null;

                var naziv = string.Format(@"{0}", Guid.NewGuid());

                string folder = Path.Combine(env.WebRootPath, "Images", "Profile");
                string nazivFajla = naziv + "." + ext;
                string filepath = Path.Combine(folder, nazivFajla);
                slika.Save(filepath);

                user.Slika = nazivFajla;
                db.SaveChanges();

                filepath = Path.Combine(folder, staraSlika);
                if(staraSlika!="avatar.png")
                System.IO.File.Delete(filepath);
                return nazivFajla;
                
            }
            catch
            {
                return null;
            }
        }
    }
}
