using CryptExApi.Data;
using CryptExApi.Models.Database;
using CryptExApi.Services;
using CryptExApi.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptExApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptExApi", Version = "v1" });

                // Authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Value format: 'Bearer %JWT_TOKEN%'"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            if (Environment.IsProduction()) {
                services.AddDbContext<CryptExDbContext>(/* Add DB Provider */);
            } else if (Environment.IsDevelopment()) {
                services.AddDbContext<CryptExDbContext>(x => x.UseInMemoryDatabase(nameof(CryptExDbContext)));

                // TODO: Mock test data
            }

            services.AddIdentity<AppUser, AppRole>(x =>
            {
                x.Password.RequiredUniqueChars = 4;
                x.Password.RequiredLength = 8;

                x.Lockout.AllowedForNewUsers = false;

                x.User.AllowedUserNameCharacters = StringUtilities.AlphabetMin + StringUtilities.AlphabetMaj + StringUtilities.Numbers;
                x.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<CryptExDbContext>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtSigningKey"));

                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,

                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

                x.Validate(JwtBearerDefaults.AuthenticationScheme);
            });

            services.AddLogging(x =>
            {
                if (Environment.IsDevelopment()) {
                    x.AddConsole();
                    x.SetMinimumLevel(LogLevel.Debug);
                } else if (Environment.IsProduction()) {
                    x.AddApplicationInsights(Configuration.GetConnectionString("AppInsights"));
                }
            });

            services.AddApplicationInsightsTelemetry();

            services.AddAuthorization();

            StripeConfiguration.ApiKey = Configuration["StripePrivateKey"];

            services.AddSingleton<IExceptionHandlerService, DefaultExceptionHandlerService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IStripeService, StripeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptExApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
