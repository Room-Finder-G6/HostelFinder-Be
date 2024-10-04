using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Mappings;
using HostelFinder.Application.Services;
using HostelFinder.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RoomFinder.Domain.Common.Settings;
using System.Text;

namespace HostelFinder.Application
{
    public class ServiceExtensions
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //register service
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthAccountService, AuthAccountService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IHostelService, HostelService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IServiceService, ServiceService>();




            //register automapper
            services.AddAutoMapper(typeof(GeneralProfile).Assembly);

            //register jwt token
            var jwtSettings = services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddSingleton(jwtSettings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy($"{UserRole.Admin}", policy => policy.RequireRole("Admin"));
                options.AddPolicy($"{UserRole.User}", policy => policy.RequireRole("User"));
            });
        }

       
    }
}
