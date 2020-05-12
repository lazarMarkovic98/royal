using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MojGrad.Models;
using MojGrad.Repo;
using MojGrad.Services;

namespace MojGrad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private IIzazov p;
        private IKorisnik u;
        private ISlika s;
        private IWebHostEnvironment env;
        public string result;
        public WeatherForecastController(IIzazov p,IKorisnik u,ISlika s,IWebHostEnvironment env)
        {
            this.p = p;
            this.u = u;
            this.s = s;
            this.env = env;
        }


        [HttpGet]
        public IActionResult get()
        {
            MojToken t = new MojToken();
            return Ok(t.dajToken(2));
        }
        private double toRadians(double x)
        {
            return (Math.PI / 180) * x;
        }



        [HttpPut]
        public IActionResult put([FromBody] SlikaIzazova slika)
        {
            //var message = s.dodajSliku(slika.PostId, slika.naziv);
            //if (message!=null)
            //return Ok(message);

            return BadRequest("greska");
        }
        [Route("laz")]
        [HttpGet]
        public IActionResult v()
        {
            return Ok("radi");
        }

        //[HttpPost]
        /*public IActionResult loadMore([FromBody] LoadMore l)
        {
           
                return Ok(p.loadMore(l.x, l.y, l.idPoslednjeg));
            
        }*/

        public class LoadMore
        {
            public long idPoslednjeg { get; set; }
            public double x { get; set; }
            public double y { get; set; }
        }
    }
}
