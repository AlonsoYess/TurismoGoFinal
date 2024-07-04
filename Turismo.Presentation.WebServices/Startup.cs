using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Turismo.Domain.Entities.Entidades;
using Turismo.Infraestructure.EFDataContext.Context;
using Turismo.Services.Implementation.Services;
using Turismo.Services.Interfaces.Interfaces;

namespace Turismo.Presentation.WebServices
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private RSA _rsa; // Variable para almacenar la clave RSA
        public Startup(IConfiguration configuration) { 
            
            Configuration = configuration;
        }
        public RSA RsaKey { get; } // Propiedad para almacenar la clave RSA

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<RSA>(RSA.Create());
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IEmpresaService, EmpresaService>();
            services.AddTransient<IActividadService, ActividadService>();
            services.AddTransient<IFavoritoService, FavoritoService>();
            services.AddTransient<IItinerarioService, ItinerarioService>();
            services.AddTransient<IReseniaService, ReseniaService>();
            services.AddTransient<IReservaService, ReservaService>();

            _rsa = RSA.Create();


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });


            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddDbContext<DBContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DevConnection"));
            });

            
            services.AddIdentity<Usuario, IdentityRole>()
                    .AddEntityFrameworkStores<DBContext>()
                    .AddDefaultTokenProviders();

            
            var builder = services.AddIdentityCore<Usuario>();
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddRoles<IdentityRole>();
            builder.AddEntityFrameworkStores<DBContext>();
            builder.AddSignInManager<SignInManager<Usuario>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(_rsa), // Utiliza la clave RSA en lugar de la clave simétrica
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Token:Audience"],
                    ValidateLifetime = false,
                    //ClockSkew = TimeSpan.Zero, // Esto fuerza la expiración exacta del token
                    //RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    TokenDecryptionKey = new RsaSecurityKey(_rsa), // Agrega la clave de descifrado
                    ValidateTokenReplay = true,
                    SaveSigninToken = true/*,
                    ValidIssuers = new[] { Configuration["Token:Issuer"] },
                    ValidAudiences = new[] { Configuration["Token:Audience"] }*/
                };
            });

        }

        public void Configure( IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("AllowAllOrigins");

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}
