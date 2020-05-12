using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PrijavaController : ControllerBase
    {
        IPrijava p;
        IIzazov post;
        IDogadjaj dogadjaj;
        IKorisnik u;

        public PrijavaController(IPrijava p, IIzazov post,IDogadjaj dogadjaj,IKorisnik u)
        {
            this.p = p;
            this.post = post;
            this.dogadjaj = dogadjaj;
            this.u = u;
        }

        [HttpGet]
        public IActionResult All()
        {

            var prijave = p.dajPrijave();
            if (prijave == null) return BadRequest();
            var prijavljeni = post.dajPrijavljeneIzazove(prijave);
            if (prijavljeni == null) return BadRequest();
            return Ok(prijavljeni);
        }
        [HttpGet("{idPosta}")]
        public IActionResult deleteReport(int idPosta)
        {
            if (p.izbrisiPrijave(idPosta) == false) return BadRequest();
            return Ok();
        }

        [HttpGet("{idPosta}")]
        public IActionResult deletePost(int idPosta)
        {
            if (p.izbrisiIzazov(idPosta) == false) return BadRequest();
            return Ok();
        }

        [HttpPost]
        public IActionResult prijaviPost([FromBody] PrijavaReq prijava)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(prijava.token);
            if (userId == -1) return Unauthorized();
            Prijava p = new Prijava();
            p.Datum = DateTime.Now;
            p.IzazovId = prijava.postId;
            p.KorisnikId = userId;
            p.Text = prijava.razlog;
            if(this.p.prijaviIzazov(p)==false) return BadRequest();
            return Ok();
        }
        [HttpPost]
        public IActionResult prijaviEvent([FromBody]PrijavaReq prijava)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(prijava.token);
            if (userId == -1) return Unauthorized();
            PrijavaDogadjaja p = new PrijavaDogadjaja();
            p.KorisnikId = userId;
            p.DogadjajId = prijava.eventId;
            p.Datum = DateTime.Now;
            p.Opis = prijava.razlog;
            if(this.p.prijaviDogadjaj(p)==true) return Ok();
            return BadRequest();
            
        }
        [HttpGet]
        public IActionResult prijaveDogadjaja()
        {
            var prijave = p.dajPrijaveDogadjaja();
            if (prijave == null) return BadRequest();
            var prijavljeni = dogadjaj.dajPrijavljeneDogadjaje(prijave);
            if (prijavljeni == null) return BadRequest();
            return Ok(prijavljeni);
        }
        [HttpPost]
        public IActionResult izbrisiPrijaveDogadjaja([FromBody] PrijavaReq prijava)
        {
            if (u.proveriAdmina(prijava.token) == -1) return Unauthorized();
            if(p.izbrisiPrijaveDogadjaja(prijava.eventId)==true) return Ok();
            return BadRequest();
        }
        [HttpPost]
        public IActionResult izbrisiDogadjaj([FromBody] PrijavaReq prijava)
        {
            if (u.proveriAdmina(prijava.token) == -1) return Unauthorized();
            if (p.izbrisiDogadjaj(prijava.eventId) == true) return Ok();
            return BadRequest();
        }
        public class PrijavaReq
        {
            public string token { get; set; }
            public long postId { get; set; }
            public string razlog {get;set;}
            public long eventId { get; set; }
        }


    }
}
