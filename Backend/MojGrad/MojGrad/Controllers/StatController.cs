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
    public class StatController : ControllerBase
    {

        IKorisnik user;
        IIzazov post;
        IDogadjaj dogadjaj;
        IKomentar komentar;
        IPrijava prijava;
        IResenje resenje;
        IKategorija kategorija;
        public StatController(IKorisnik u,IIzazov p,IDogadjaj e,IKomentar k,IPrijava prijava,IResenje resenje,IKategorija kategorija)
        {
            this.user = u;
            this.post = p;
            this.dogadjaj = e;
            this.komentar = k;
            this.prijava = prijava;
            this.resenje = resenje;
            this.kategorija = kategorija;
        }
        [HttpGet]
        public IActionResult Count()
        {
            var res = new
            {
                korisnici = user.brojKorisnika(),
                postovi = post.brojIzazova(),
                dogadjaji = dogadjaj.brojDogadjaja(),
                komentari = komentar.brojKomentara(),
                prijave = prijava.brojPrijava(),
                resenja = resenje.brojResenja()
            };
            return Ok(res);
        }
        [HttpGet]
        public IActionResult solvedUnsolved()
        {
            var t = post.brojResenihINeresenih();
            var res = new
            {
                reseni = t.Item1,
                nereseni = t.Item2
            };
            return Ok(res);
        }
        [HttpGet]
        public IActionResult eventLastMonth()
        {
            return Ok(dogadjaj.dogadjajiUPoslednjih30Dana());
        }
        [HttpGet]
        public IActionResult postLastMonth()
        {
            return Ok(post.izazoviUPoslednjih30Dana());
        }
        [HttpGet]
        public IActionResult solutionLastMonth()
        {
            return Ok(resenje.resenjaUPoslednjih30Dana());
        }
        [HttpGet]
        public IActionResult categoryPost()
        {
            return Ok(kategorija.izazoviPoKategorijama());
        }
    }
}