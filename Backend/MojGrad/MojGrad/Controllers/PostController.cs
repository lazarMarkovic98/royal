using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {

        private IIzazov p;
        private ISlika s;
        private IKorisnik u;
        public PostController(IIzazov p,ISlika s,IKorisnik u)
        {
            this.p = p;
            this.s = s;
            this.u = u;
        }

        //vrati postove u blizini koordinata x i y
        [HttpGet("{x}/{y}")]
        public IActionResult Get(double x,double y)
        {
           
            var postovi = p.dajPostovePocetne(x, y);
            
            

            return Ok(postovi);
        }
        [HttpGet("{x}/{y}/{id}/{radius}")]
        public IActionResult getAsync(double x,double y,long id,double radius)
        {
            //return Ok(radius);
            return Ok(p.ucitajIzazove(x, y, id,radius));
        }
        
        [HttpGet("solved/{x}/{y}/{id}")]
        public IActionResult getSolved(double x, double y, long id)
        {
            return Ok(p.loadSolved(x,y,id));
        }

        [HttpGet("unsolved/{x}/{y}/{id}")]
        public IActionResult getUnsolved(double x, double y, long id)
        {
            return Ok(p.loadUnsolved(x, y, id));
        }

        //vrati detalje posta
        [HttpGet("{idPosta}")]
        public IActionResult Get(long idPosta)
        {

            var detalji = p.dajIzazov(idPosta);



            return Ok(detalji);
        }

        [HttpPost]
        public IActionResult Add([FromForm] addPost req)
        {
            MojToken t = new MojToken();
            var id = t.verifikujToken(req.token);
            if (id == -1) return Unauthorized();
            var ban = u.jeOpomenut(id);
            if (ban != default(DateTime)) return BadRequest(new { message = ban });
            Izazov post = new Izazov();
            post.Datum = DateTime.Now;
            post.KategorijaId = req.idKategorije;
            post.KorisnikId = id;
            post.X = req.x;
            post.Y = req.y;
            post.Opis = req.opis;
            post.Naslov = req.naslov;
            long idPosta = p.dodaj(post);

            String a = null;
            
            if (idPosta == -1)
            {
                return BadRequest();
            }
            var fajl = req.img1;
            var ms = new MemoryStream();
            fajl.CopyTo(ms);
            Image img = Image.FromStream(ms,true);
            a = s.dodajSliku(idPosta, img);
            if (req.img2 != null)
            {
                var fajl2 = req.img2;
                var ms2 = new MemoryStream();
                fajl2.CopyTo(ms2);
                Image img2 = Image.FromStream(ms2, true);
               
                var x = s.dodajSliku(idPosta, img2);
                if (x != null) a = x;

            }

            if (req.img3 != null)
            {
                var fajl3 = req.img3;
                var ms3 = new MemoryStream();
                fajl3.CopyTo(ms3);
                Image img3 = Image.FromStream(ms3, true);
                var x = s.dodajSliku(idPosta, img3);
                if (x != null) a = x;

            }

            if (req.img4 != null)
            {
                var fajl4 = req.img4;
                var ms4 = new MemoryStream();
                fajl4.CopyTo(ms4);
                Image img4 = Image.FromStream(ms4, true);
                var x = s.dodajSliku(idPosta, img4);
                if (x != null) a = x;

            }
            if (a == null) return Ok();

            p.obrisi(idPosta);
            return BadRequest();
            
        }

        [Route("edit")]
        [HttpPost]
        public IActionResult editPost([FromBody] EditPost post)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(post.token);
            if (userId == -1) return Unauthorized();
            if (p.izmeni(post.idPosta, post.naslov, post.opis) == false) return BadRequest();
            return Ok();
        }


        [Route("delete")]
        [HttpPost]
        public IActionResult deletePost([FromBody] EditPost post)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(post.token);
            if (userId == -1) return Unauthorized();
            if (p.obrisi(post.idPosta) == false) return BadRequest();
            return Ok();
        }
        [HttpGet("loadadmin/{idPoslednjeg}")]
        public IActionResult getAdmin(long idPoslednjeg)
        {
            return Ok(p.dajIzazoveAdminStrane(idPoslednjeg));
        }
        public class addPost
        {
            public string opis { get; set; }
            public int idKategorije { get; set; }
            public string naslov { get; set; }
            public float x { get; set; }
            public float y { get; set; }
            public string token { get; set; }
            public IFormFile img1 { get; set; }
            public IFormFile img2 { get; set; }
            public IFormFile img3 { get; set; }
            public IFormFile img4 { get; set; }
        }
        public class EditPost
        {
            public long idPosta { get; set; }
            public string token { get; set; }
            public string opis { get; set; }
            public string naslov { get; set; }
        }
       
    }
}