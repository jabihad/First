using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FileUploadApi.JwtFeatures;
using FileUploadApi.Repositories;
using FileUploadApi.Services.AppUser.Interfaces;
using FileUploadApi.Services.AppUser.Implementation;
using FileUploadApi.Services.Upload.Interfaces;
using FileUploadApi.Services.Upload.Implementation;
using FileUploadApi.Services.Login.Interfaces;
using FileUploadApi.Services.Login.Implementation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using FileUploadApi.Services.Report.Interfaces;
using FileUploadApi.Services.Report.Implementation;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace FileUploadApi
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddDbContext<RepositoryContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"))

            );
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FileUploadApi", Version = "v1" });
            });
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<RepositoryContext>();

            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });
            //
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = null;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = null;
            });
            //
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<JwtHandler>();

            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IReportService, ReportService>();

            services.AddHttpContextAccessor(); 
            services.AddSession();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileUploadApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

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
