using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestfulAPI.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestfulAPI.Mappings;
using RestfulAPI.Repository;
using RestfulAPI.Repository.IRepository;
using System.Reflection;
using System.IO;

namespace RestfulAPI
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<INationalParkRepository,NationalParkRepository>();
            services.AddAutoMapper(typeof(NationalParkMappings));
            services.AddAutoMapper(typeof(TrailsMappings));
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("RestfulOpenApiSpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "RESTFUL API",
                    Version = "1.0",
                    Description = "RESTFUL API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Omkar Navik",
                        Email = "Navik46@gmail.com",
                        Url = new Uri("https://github.com/omkar178/RestfulAPI/tree/master/RestfulAPI")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "GIT License",
                        Url = new Uri("https://github.com/omkar178/RestfulAPI"),

                    }
                });
                var xmlCommentFilePath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var fullPath = Path.Combine(AppContext.BaseDirectory,xmlCommentFilePath);
                options.IncludeXmlComments(fullPath);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/RestfulOpenApiSpecification/swagger.json", "RestfulAPI");
                options.RoutePrefix = "";
               
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
