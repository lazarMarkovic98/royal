using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MojGrad.Models;

namespace MojGrad.Services
{
    public class Context : DbContext
    {
        public DbSet<Dogadjaj> Dogadjaji { get; set; }
        public DbSet<Kategorija> Kategorije { get; set; }
        public DbSet<Komentar> Komentari { get; set; }
        public DbSet<Nadleznost> Nadleznost { get; set; }
        public DbSet<Izazov> Izazovi { get; set; }

        public DbSet<SlikaIzazova> SlikeIzazova { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }

        public DbSet<Prijava> Prijave { get; set; }
        
        public DbSet<Resenje> Resenja { get; set; }
        public DbSet<SlikaResenja> SlikeResenja { get; set; }
        public DbSet<Ucesnik> Ucesnici { get; set; }
        public DbSet<KomentarDogadjaja> KomentariDogadjaja { get; set; }
        public DbSet<Medalja> Medalje { get; set; }
        public DbSet<DodeljenaMedalja> DodeljeneMedalje { get; set; }
        public DbSet<PrijavaDogadjaja> PrijaveDogadjaja { get; set; }
        public Context(DbContextOptions<Context> options)
        : base(options)
        { }

    }
}
