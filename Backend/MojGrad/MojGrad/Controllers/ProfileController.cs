using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MojGrad.Interfaces;
using MojGrad.Models;
using MojGrad.Services;

namespace MojGrad.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {

        IKorisnik u;
        IIzazov p;
        ISlika s;
        IDogadjaj e;
        IMedalja m;
        IResenje r;
        public ProfileController(IKorisnik u,IIzazov p,ISlika s,IDogadjaj e,IMedalja m,IResenje r)
        {
            this.u = u;
            this.p = p;
            this.s = s;
            this.e = e;
            this.m = m;
            this.r = r;
        }

        //Vrati informacije za header profila
        //Dodati broj poena u model
        //Impleentirati sistem pracenja
        [HttpGet("{id}")]
        public IActionResult header(int id)
        {
            var res = u.dajKorisnika(id);
            return Ok(new
            {
                Id = res.Id,
                Image = URL.ProfileUrl + res.Slika,
                Poeni = res.Poeni,
                Bio = res.Bio,
                Username = res.KorisnickoIme
            });
        }


        [HttpGet("{id}")]
        public IActionResult posts(long id)
        {
            var res = p.dajIzazoveKorisnika(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public IActionResult events(long id)
        {
            var res = e.dajDogadjajeKorisnika(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public IActionResult medals(long id)
        {
            var res = m.dajMedaljeKorisnika(id);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public IActionResult resenja(long id)
        {
            var res = r.dajResenjaKorisnika(id);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public IActionResult username(int id)
        {
            var res = u.dajKorisnika(id);
            return Ok(new {username=res.KorisnickoIme });
        }
        [HttpPost]
        public IActionResult changeUsername([FromBody] Profile p)
        {
            MojToken token = new MojToken();
            var id = token.verifikujToken(p.token);
            if (id == -1) return Unauthorized();
            var t = u.promeniKorisnickoIme(id, p.username);
            if (t == true) return Ok(new { username = p.username });
            else return BadRequest();
        }
        [HttpPost]
        public IActionResult changePassword([FromBody] Profile p)
        {
            MojToken token = new MojToken();
            var id = token.verifikujToken(p.token);
            if (id == -1) return Unauthorized();
            var t = u.promeniLozinku(id, p.password,p.oldPassword);
            if (t == true) return Ok();
            else return BadRequest();
        }

        [HttpPost]
        public IActionResult changeImage([FromBody] Profile p)
        {
            MojToken token = new MojToken();
            var id = token.verifikujToken(p.token);
            if (id == -1) return Unauthorized();
            var t = s.promeniSlikuProfila(id, p.image);
            if (t == null) return BadRequest();
            return Ok(new {image=URL.ProfileUrl+t });
        }
        [HttpPost]
        public IActionResult changeBio([FromBody] Profile p)
        {
            MojToken token = new MojToken();
            var id = token.verifikujToken(p.token);
            if (id == -1) return Unauthorized();
            var t = u.promeniBio(id, p.bio);
            if (t == true) return Ok();
            else return BadRequest();
        }

        public IActionResult changeRadius([FromBody] Profile p)
        {
            MojToken token = new MojToken();
            var id = token.verifikujToken(p.token);
            if (id == -1) return Unauthorized();
            var t = u.promeniRadius(id, p.radius);
            if (t == true) return Ok();
            else return BadRequest();

        }
        [HttpPost]
        public IActionResult passwordRecovery([FromBody] Profile p)
        {
            MojToken t = new MojToken();
            var userId = t.verifikujToken(p.token);
            if (userId == -1) return Unauthorized();
            var a = u.promeniLozinku(userId, p.password);
            if (a == true) return Ok();
            else return BadRequest();
        }
        public class Profile
        {
            public string token { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string oldPassword { get; set; }
            public string image { get; set; }
            public string bio { get; set; }
            public double radius { get; set; }
        }
    }
}