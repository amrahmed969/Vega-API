using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vega_API.Core;
using Vega_API.Persisitance;

namespace Vega_API
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
           
            services.AddCors();

           services.AddAutoMapper();

            //add  JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://dev-aoletybq.us.auth0.com/";
                options.Audience = "https://api.vega.com";
            });


            services.AddControllers();
            //add Db Context
            services.AddDbContext<VegaDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("Default")));
            // add repository
           services.AddScoped<IVehicleRepository, VehicleRepository>();
            //add UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //using swager 
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Place Info Service API",
                    Version = "v2",
                    Description = "Sample service for Learner",
                });
            });          

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              
            }
            // using swager//  
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "PlaceInfo Services"));


            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                       
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
