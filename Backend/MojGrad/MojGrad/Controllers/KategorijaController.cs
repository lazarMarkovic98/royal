using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MojGrad.Models;

namespace MojGrad.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KategorijaController : ControllerBase
    {
        IKategorija k;
        IIzazov p;
        public KategorijaController(IKategorija k,IIzazov p)
        {
            this.k = k;
            this.p = p;
        }

        [HttpGet]
        public IActionResult get()
        {
            
            return Ok(k.dajKategorije());
        }


        [Route("postovi/{id}")]
        public IActionResult getPosts(int id)
        {
            var res = p.dajIzazoveKategorije(id);
            if (res == null) return BadRequest();
            return Ok(res);
        }
    }
}