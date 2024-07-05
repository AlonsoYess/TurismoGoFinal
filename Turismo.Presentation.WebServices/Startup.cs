using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;
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

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder => builder
                        .WithOrigins("http://localhost:9000") 
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
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

            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                       ClockSkew = TimeSpan.Zero
                   }
               );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Configurar la seguridad de Swagger para JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Configurar el requerimiento de seguridad para Swagger
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
                        new string[] { }
                    }
                });
            });

        }

        public void Configure( IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Imagenes")),
                RequestPath = "/Imagenes"
            });

            app.UseRouting();

            app.UseCors("AllowSpecificOrigins");

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}
