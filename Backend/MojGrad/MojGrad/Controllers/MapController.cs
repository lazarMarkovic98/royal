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
    public class MapController : ControllerBase
    {
        IIzazov p;
        public  MapController(IIzazov p)
        {
            this.p = p;
        }
        [HttpGet("{x}/{y}")]
        public IActionResult Get(double x,double y)
        {
            return Ok(p.dajKoordinate(x, y));
        }
    }
}