using System;
using System.IO;
using System.Text;
using DAL.Data;
using DAL.Factory;
using DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace OppifonAPI
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
            var connectionString = "SQLCONNSTR_ConnectionString";

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(connectionString)));

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,
                    //ValidIssuer = "the isser you want to validate",

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AKSE5aYnyjEXs5eSQbYrHdaW6QEXWXGfxKfe9MxhPxyFpg2bghzFNAX7Wu4xrhExeZFBdm6Qzz85sDMWptWWJp7Jz6pwr9w2GTeP3MJer7M8kjKkzZWcdBGJ")),

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromHours(5) //5 minute tolerance for the expiration date
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Oppifon API", Version = "v1"
                });
            });

            services.AddCors();

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddMvc();
            services.AddSingleton<IFactory, Factory>();

            Factory.ConnectionString = Configuration.GetConnectionString(connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Oppifon API V1");
            });

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            DefaultFilesOptions defaultFile = new DefaultFilesOptions();
            defaultFile.DefaultFileNames.Clear();
            defaultFile.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFile);
            app.UseStaticFiles();

            app.UseAuthentication();
            
            app.UseMvc();
        
        }
    }
}
