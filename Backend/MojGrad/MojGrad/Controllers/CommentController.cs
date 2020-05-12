using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MojGrad.Models;

namespace MojGrad.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private IKomentar k;
        private IKorisnik u;

        public CommentController(IKomentar k,IKorisnik u)
        {
            this.k = k;
            this.u = u;
        }

        [HttpGet("{id}")]
        public IActionResult like(int id)
        {
            if (k.lajkIzazov (id) == true) return Ok();
            else return BadRequest();
        }

        [HttpGet("{id}")]
        public IActionResult dislike(int id)
        {
            if (k.dislajkIzazov(id) == true) return Ok();
            else return BadRequest();
        }

        [HttpPost]
        public IActionResult add([FromBody] Komentar komentar)
        {
            var ban = u.jeOpomenut(komentar.KorisnikId);
            if (ban != default(DateTime)) return BadRequest(new { message = ban });
            if (k.dodajIzazov(komentar) == true) return Ok();
            return BadRequest();
        }

        [HttpGet("{id}")]
        public IActionResult likeEvent(int id)
        {
            if (k.lajkDogadjaj(id) == true) return Ok();
            else return BadRequest();
        }

        [HttpGet("{id}")]
        public IActionResult dislikeEvent(int id)
        {
            if (k.dislajkDogadjaj(id) == true) return Ok();
            else return BadRequest();
        }
        
        [HttpPost]
        public IActionResult addEvent([FromBody] KomentarDogadjaja komentar)
        {
            var ban = u.jeOpomenut(komentar.KorisnikId);
            if (ban != default(DateTime)) return BadRequest(new { message = ban });
            if (k.dodajDogadjaj(komentar) == true) return Ok();
            return BadRequest();
        }
    }
}