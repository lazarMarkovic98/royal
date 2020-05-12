using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MojGrad.Models;
using MojGrad.Repo;
using MojGrad.Services;
using MojGrad.Interfaces;
using System.Collections.Specialized;
using Quartz.Impl;
using Quartz;
using System.Diagnostics;

namespace MojGrad
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(options => options.UseSqlite("Data Source=Baza.db"));
            services.AddTransient<IKorisnik, KorisnikRepo>();
            services.AddTransient<IIzazov, IzazovRepo>();
            services.AddTransient<ISlika, SlikaRepo>();
            services.AddTransient<IKomentar, KomentarRepo>();
            services.AddTransient<IKategorija, KategorijaRepo>();
            services.AddTransient<IPrijava, PrijavaRepo>();
            services.AddTransient<IDogadjaj, DogadjajRepo>();
            services.AddTransient<IResenje, ResenjeRepo>();
            services.AddTransient<IMedalja, MedaljaRepo>();

            Test();

            services.AddControllers();



            services.AddMvc();
        }

        private void Test()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            IJobDetail medaljaJob = JobBuilder.Create<MedaljaRepo>()
           .WithIdentity("MedaljaJob")
           .Build();
            ITrigger medaljaTrigger = TriggerBuilder.Create()
                .ForJob(medaljaJob)
                .WithCronSchedule("0 0 0 1 * ?")
                .WithIdentity("MedaljaTrigger")
                .StartNow()
                .Build();
            scheduler.ScheduleJob(medaljaJob, medaljaTrigger);
            scheduler.Start();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>

            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();

                

            }

            );


            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStaticFiles();
        }
    }
}
