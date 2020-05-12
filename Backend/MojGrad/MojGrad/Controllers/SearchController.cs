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
    public class SearchController : ControllerBase
    {
        IDogadjaj e;
        IIzazov p;
        IKorisnik u;

        public SearchController(IDogadjaj e,IIzazov p,IKorisnik u)
        {
            this.u = u;
            this.p = p;
            this.e = e;
        }
        [HttpPost]
        public IActionResult search([FromBody] SearchClass s)
        {
            MojToken t = new MojToken();
            if (t.verifikujToken(s.token) == -1) return Unauthorized();
            var users = u.pretraga(s.search);
            var events = e.pretraga(s.search);
            var posts = p.pretraga(s.search);
            if (users == null || events == null || posts==null)
            {
                BadRequest();
            }
            return Ok(new
            {
                users = users.Select(x => new
                {
                    username = x.KorisnickoIme,
                    ime = x.Ime,
                    prezime = x.Prezime,
                    id = x.Id,
                    slika = URL.ProfileUrl + x.Slika
                }),
                posts = posts,
                events = events
            });
        }

        public class SearchClass
        {
            public string token { get; set; }
            public string search { get; set; }
        }
    }
}