using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailService.Application.EmailSender;
using MailService.Models;
using MailService.RabbitMQ;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.Generation.Processors.Security;
using Steeltoe.Discovery.Client;

namespace MailService
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
            services.AddSingleton<IHostedService>(provider => new Consumer("AccountToEmail"));
            services.AddSingleton<IHostedService>(provider => new Consumer("CreateCampaign"));
            services.AddSingleton<IHostedService>(provider => new Consumer("CreateTask"));
            services.AddSingleton<IHostedService>(provider => new Consumer("DedlineTask"));
            //Addservice mail
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            //add swagger
            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = string.Format($"MailService Service");
                    document.Info.Description = string.Format($"Developer Documentation Page For MaiService Service");
                };
                config.DocumentProcessors.Add(new SecurityDefinitionAppender("Jwt Token Authentication", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Using: Bearer + your jwt token",
                    In = OpenApiSecurityApiKeyLocation.Header
                }));
            });
            //MediatR and flutentvalidator
            services.AddRouting(o => o.LowercaseUrls = true);
            //services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetCampaignValidator>());
            //services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCampaignCommand>());
            //add ueraka
            services.AddDiscoveryClient(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            //addd cors
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            //addservice
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

             //app.UseDeveloperExceptionPage();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseDiscoveryClient();
            app.UseMvc();
        }
    }
}
