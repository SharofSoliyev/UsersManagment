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
using UsersManagment.Infostructure.Data;
using Microsoft.EntityFrameworkCore;
using UsersManagment.Businees.Settings;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace UsersManagment
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
            services.Configure<TokenSetting>(Configuration.GetSection(nameof(TokenSetting)));
            ConfigureJwtAuthentication(services, Configuration);
            ConfigureUserManagmentServices(services,Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UsersManagment", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization Bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void ConfigureUserManagmentServices(IServiceCollection services, IConfiguration Configuration)
        {
            ConfigureDatabases(services, Configuration);

        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsersManagment v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public static void ConfigureJwtAuthentication(IServiceCollection services, IConfiguration Configuration)
        {

            var key = Encoding.ASCII.GetBytes(Configuration["TokenSetting:Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireSignedTokens = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = 401;
                        //context.Response.Body = new ApiResponse(401, "");
                        context.Response.Headers.Append("my-custom-header", "custom-value");
                        await context.Response.WriteAsync("You are not authorized");
                    }
                };
            });
        }
        public static void ConfigureDatabases(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<AppDataContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("UsersManagmentString"))
                .EnableSensitiveDataLogging();
            });
        }
    }
}
