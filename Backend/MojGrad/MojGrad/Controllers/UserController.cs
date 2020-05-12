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
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private IKorisnik u;
        public UserController(IKorisnik u)
        {
            this.u = u;
        }
        //Prikazi sve korisnike iz baze
        [HttpGet("adminUser")]
        public IActionResult Get()
        {
            return Ok(u.dajSveKorisnike());
        }

        //Logovanje/registracija
        [HttpPost]
        public IActionResult Login([FromBody] LoginUser user)
        {
            MojToken t = new MojToken();
            if (user.Ime==null)
            {
                
                Korisnik korisnik = u.proveriKorisnika(user.Username, user.Password);
                
                if (korisnik == null)
                    return BadRequest();
                u.osveziFcmToken(user.fcmToken, korisnik.Id);
                korisnik.Lozinka = null;
                korisnik.Slika = URL.ProfileUrl + korisnik.Slika;
                return Ok(new
                {
                    username = korisnik.KorisnickoIme,
                    id=korisnik.Id,
                    imePrezime = korisnik.Ime+" "+korisnik.Prezime,
                    img = korisnik.Slika,
                    token = t.dajToken(korisnik.Id),
                    radius = (double)korisnik.Radius
                });
            }
            else
            {
                Korisnik korisnik = u.registrujKorisnika(user.Username, user.Password, user.Ime, user.Prezime, user.Email,user.fcmToken);
                if (korisnik == null) return BadRequest();
                korisnik.Lozinka = null;
                korisnik.Slika = URL.ProfileUrl + korisnik.Slika;
                return Ok(new
                {
                    username = korisnik.KorisnickoIme,
                    id = korisnik.Id,
                    imePrezime = korisnik.Ime + " " + korisnik.Prezime,
                    img = korisnik.Slika,
                    token = t.dajToken(korisnik.Id),
                    radius=korisnik.Radius
                });
            }
                 
        }

        [Route("resetpass")]
        [HttpPost]
        public IActionResult get([FromBody] LoginUser user)
        {
            Korisnik korisnik=null;
            if (user.Email != null)
            {
               korisnik =  u.nadjiKorisnika(null, user.Email);
            }
            if (user.Username != null)
            {
                korisnik = u.nadjiKorisnika(user.Username, null);
            }
            if (korisnik == null) return BadRequest();

            MailService m = new MailService();
            m.sendRecoveryMail(korisnik);
            return Ok();
        }
        [Route("unapredi")]
        [HttpPost]
        public IActionResult unapredi([FromBody]AdminUser req)
        {
            var adminId=u.proveriAdmina(req.token);
            if (adminId == -1) return Unauthorized();
            if (u.unapredi(req.userId) == false) return BadRequest();
            return Ok();
        }

        [Route("institucija")]
        [HttpPost]
        public IActionResult dodajInstituciju([FromBody]Institucija req)
        {
            if (u.proveriAdmina(req.Token) == -1) return Unauthorized();
            var institucija =u.dodajInstituciju(req.Username, req.Password, req.Naziv, req.Email,req.Kategorije);
            if (institucija == null) return BadRequest();
            return Ok();
        }

        [Route("loginweb")]
        [HttpPost]
        public IActionResult loginWeb([FromBody]LoginUser req)
        {
            var user = u.prijavaWeb(req.Username, req.Password);
            if (user == null) return BadRequest();
            return Ok(user);
        }
        [Route("opomeni")]
        [HttpPost]
        public IActionResult opomeni([FromBody]AdminUser req)
        {
            if (u.proveriAdmina(req.token) == -1) return Unauthorized();
            if (u.opomeni(DateTime.Parse(req.datum), req.userId) == true) return Ok();
            return BadRequest();
        }
        public class LoginUser
        {
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string fcmToken { get; set; }
        }
        public class AdminUser
        {
            public string token { get; set; }
            public long userId { get; set; }
            public string datum { get; set; }
        }
        public class Institucija
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Naziv { get; set; }
            public string Token { get; set; }
            public List<long> Kategorije { get; set; }
        }

    }

    

    
}