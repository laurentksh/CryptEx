using Coinbase;
using CryptExApi.Data;
using CryptExApi.Models.Database;
using CryptExApi.Models.SignalR;
using CryptExApi.Repositories;
using CryptExApi.Services;
using CryptExApi.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptExApi
{
    public class Startup
    {
        private const string CorsPolicyName = "CorsPolicy";

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
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptExApi", Version = "v1" });

                // Authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Value format: '%JWT_TOKEN%'"
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

            services.AddCors(x =>
            {
                x.AddPolicy(CorsPolicyName, y =>
                {
                    y.AllowAnyMethod();
                    y.AllowAnyHeader();
                    y.AllowCredentials();
                    //y.AllowAnyOrigin(); //We allow any origin because we aren't a real website.
                    y.WithOrigins(
                        "http://localhost:5000",
                        "https://localhost:5001",
                        "http://localhost:4200",
                        "https://cryptex-trade.tech",
                        "https://www.cryptex-trade.tech",
                        "https://api.cryptex-trade.tech"
                    );
                  //y.WithOrigins("cryptex-trade.tech", "www.cryptex-trade.tech");
                });
            });
            
            services.AddDbContext<CryptExDbContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("Database"), options =>
                {
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });

            services.AddIdentity<AppUser, AppRole>(x =>
            {
                x.Password.RequiredUniqueChars = 4;
                x.Password.RequiredLength = 8;
                x.Password.RequireNonAlphanumeric = false;

                x.Lockout.AllowedForNewUsers = false;

                x.User.AllowedUserNameCharacters = StringUtilities.AlphabetMin + StringUtilities.AlphabetMaj + StringUtilities.Numbers;
                x.User.RequireUniqueEmail = true;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<CryptExDbContext>();

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
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,

                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

                x.Validate(JwtBearerDefaults.AuthenticationScheme);
            });

            if (Environment.IsProduction())
                services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddLogging(x =>
            {
                if (Environment.IsDevelopment()) {
                    x.AddConsole();
                    x.SetMinimumLevel(LogLevel.Debug);
                } else if (Environment.IsProduction()) {
                    x.AddApplicationInsights(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
                }
            });

            services.AddAuthorization();

            StripeConfiguration.ApiKey = Configuration["StripePrivateKey"];

            services.AddTransient<ICoinbaseClient, CoinbaseClient>();

            services.AddTransient<IDepositRepository, DepositRepository>();
            services.AddTransient<IStripeRepository, StripeRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IWalletRepository, WalletRepository>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IDepositService, DepositService>();
            services.AddSingleton<IExceptionHandlerService, DefaultExceptionHandlerService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IStripeService, StripeService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWalletService, WalletService>();

            if (Environment.IsDevelopment())
                services.AddTransient<IDataSeeder, DevelopmentDataSeeder>();
            else if (Environment.IsProduction())
                services.AddTransient<IDataSeeder, ProductionDataSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope()) {
                Task.Run(async () =>
                {
                    if (env.IsProduction()) {
                        var dbContext = scope.ServiceProvider.GetRequiredService<CryptExDbContext>();

                        var pending = await dbContext.Database.GetPendingMigrationsAsync();
                        if (pending.Count() == 1 && pending.Contains("Initial")) //Means we deleted all existing migrations.
                            await dbContext.Database.EnsureDeletedAsync();

                        await dbContext.Database.MigrateAsync();
                    }

                    await DefaultDataSeeder.Seed(scope.ServiceProvider);

                    await scope.ServiceProvider.GetService<IDataSeeder>()?.Seed(scope.ServiceProvider);
                }).Wait();
            }

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsPolicyName);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptExApi v1");
                c.DisplayRequestDuration();
                c.EnableValidator();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DepositHub>("/feed/deposits");
            });
        }
    }
}
