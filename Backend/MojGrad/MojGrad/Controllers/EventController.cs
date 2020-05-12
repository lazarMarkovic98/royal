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
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        IDogadjaj e;
        ISlika s;
        IKorisnik u;
        public EventController(IDogadjaj e,ISlika s,IKorisnik u)
        {
            this.e = e;
            this.s = s;
            this.u = u;
        }

        [HttpGet("{x}/{y}/{radius}")]
        public IActionResult getEvents(double x,double y,double radius)
        {
            var res = e.dajDogadjaje(x, y,radius);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public IActionResult getEvent(long id)
        {
            var res = e.dajDetalje(id);
            return Ok(res);
        }

        [HttpPut]
        public IActionResult dodajEvent([FromBody] eventModel pe)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(pe.token);
            if (userId == -1) return Unauthorized();
            var ban = u.jeOpomenut(userId);
            if (ban != default(DateTime)) return BadRequest(new { message = ban });
            Dogadjaj e = new Dogadjaj();
            e.KorisnikId = userId;
            e.Naslov = pe.naslov;
            e.Opis = pe.opis;
            var vreme = DateTime.Parse(pe.vreme);
            e.Datum = DateTime.Parse(pe.datum);
            e.Datum=e.Datum.AddHours(vreme.Hour);
            e.Datum=e.Datum.AddMinutes(vreme.Minute);
            e.X = pe.x;
            e.Y = pe.y;
            var nazivSlike=s.dodajSlikuEventa(pe.slika);
            if (nazivSlike == null) return BadRequest();
            e.Slika = nazivSlike;
            if (this.e.dodajDogadjaj(e) == false) return BadRequest();
            
            return Ok();
        }
        [Route("addinterested")]
        [HttpPost]
        public IActionResult zainteresovan([FromBody]eventModel e)
        {
            MojToken t = new MojToken();
            var idUsera = t.verifikujToken(e.token);
            if (idUsera == -1)
            {
                return Unauthorized();
            }
            Ucesnik u = new Ucesnik();
            u.KorisnikId = idUsera;
            u.DogadjajId = e.idEventa;
            if (this.e.zainteresovan(u) == true) return Ok();
            return BadRequest();
        }

        [Route("checkinterested")]
        [HttpPost]
        public IActionResult proveri([FromBody]eventModel e)
        {
            MojToken t = new MojToken();
            var idUsera = t.verifikujToken(e.token);
            if (idUsera == -1)
            {
                return BadRequest();
            }
           
            if (this.e.proveriPrisustvo(idUsera,e.idEventa) == true) return Ok();
            return BadRequest();
        }

        [Route("interestedusers")]
        [HttpPost]
        public IActionResult dajzainteresovane([FromBody]eventModel e)
        {
 
            return Ok(this.e.dajZainteresovane(e.idEventa));
        }
        
        [Route("myinterests")]
        [HttpPost]
        public IActionResult dogadjajiIdem([FromBody]eventModel e)
        {
            var t = new MojToken();
            var userId =t.verifikujToken(e.token);
            if (userId == -1)
            {
                return Unauthorized();
            }
            return Ok(this.e.dogadjajiIdem(userId));
        }
        [HttpPost]
        [Route("edit")]
        public IActionResult izmeni([FromBody]eventModel e)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(e.token);
            if (userId == -1) return Unauthorized();

            if (this.e.izmeni(e.idEventa, e.naslov, e.opis, e.datum, e.vreme) == true) return Ok();
            return BadRequest();
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult obrisi([FromBody]eventModel e)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(e.token);
            if (userId == -1) return Unauthorized();
            if (this.e.obrisi(e.idEventa) == true) return Ok();
            return BadRequest();
        }

        public class eventModel
        {
            public long idEventa { get; set; }
            public string token { get; set; }
            public string datum { get; set; }
            public string vreme { get; set; }
            public string opis { get; set; }
            public string naslov { get; set; }
            public string slika { get; set; }
            public double x { get; set; }
            public double y { get; set; }
        }
    }
}