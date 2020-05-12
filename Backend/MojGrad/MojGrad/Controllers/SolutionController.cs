using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MojGrad.Interfaces;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        ISlika s;
        IResenje r;
        IMedalja m;
        IKorisnik u;
        public SolutionController(ISlika s,IResenje r,IMedalja m,IKorisnik u)
        {
            this.s = s;
            this.r = r;
            this.m = m;
            this.u = u;
        }

        [Route("add")]
        [HttpPost]
        public IActionResult add([FromForm] PutSolution req)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(req.token);
            if (userId == -1) return Unauthorized();
            var ban = u.jeOpomenut(userId);
            if (ban != default(DateTime)) return BadRequest(new { message = ban });
            Resenje r = new Resenje();
            r.Datum = DateTime.Now;
            r.Ocena = 0;
            r.Opis = req.opis;
            r.IzazovId = req.idPosta;
            r.KorisnikId = userId;

            var idResenja = this.r.dodajResenje(r);
            if (idResenja == -1)
            {
                return BadRequest();
            }
            string a = null;
            var fajl = req.img1;
            var ms = new MemoryStream();
            fajl.CopyTo(ms);
            Image img = Image.FromStream(ms, true);
            a = s.dodajSlikuResenja(idResenja, img);

            if (req.img2 != null)
            {
                var fajl2 = req.img2;
                var ms2 = new MemoryStream();
                fajl2.CopyTo(ms2);
                Image img2 = Image.FromStream(ms2, true);

                var x = s.dodajSlikuResenja(idResenja, img2);
                if (x != null) a = x;
            }

            if (req.img3 != null)
            {
                var fajl3 = req.img3;
                var ms3 = new MemoryStream();
                fajl3.CopyTo(ms3);
                Image img3 = Image.FromStream(ms3, true);
                var x = s.dodajSlikuResenja(idResenja, img3);
                if (x != null) a = x;

            }
            if (req.img4 != null)
            {
                var fajl4 = req.img4;
                var ms4 = new MemoryStream();
                fajl4.CopyTo(ms4);
                Image img4 = Image.FromStream(ms4, true);
                var x = s.dodajSlikuResenja(idResenja, img4);
                if (x != null) a = x;
            }
            if (a == null)
            {
                m.dodeliMedaljuZaPoene(userId);
                this.r.notifikacijaResenje(req.idPosta);
                return Ok();
            }
                  

            this.r.obrisiResenje(idResenja);
            return BadRequest();
            
        }

        [HttpGet("{idPosta}")]
        public IActionResult get(long idPosta)
        {
            var res = r.dajResenja(idPosta);
            if (res == null) return BadRequest();
            return Ok(res);
        }
        
        
        [HttpPost]
        [Route("rate")]
        public IActionResult oceni([FromBody] ReqOceni req)
        {
            MojToken t = new MojToken();
            if (t.verifikujToken(req.token) == -1) return Unauthorized();
            if (r.oceni(req.idResenja, req.ocena) == false) return BadRequest();
            r.notifikacijaOcena(req.idResenja);
            return Ok();
        }
        public class ReqOceni
        {
            public long idResenja { get; set; }
            public string token { get; set; }
            public int ocena { get; set; }

        }
        public class PutSolution
        {
            public long idPosta { get; set; }
            public string token { get; set; }
            public string opis { get; set; }
            public IFormFile img1 { get; set; }
            public IFormFile img2 { get; set; }
            public IFormFile img3 { get; set; }
            public IFormFile img4 { get; set; }

        }

    }
}