using BookStore.Domain.Helpers;
using BookStore.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace BookStore.Infrastucture
{
    public static class ServiceRegisteration
    {
        public static IServiceCollection AddServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
        {

            #region Identity Configurations

            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                // Password settings.
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<BookDBContext>().AddDefaultTokenProviders();

            #endregion

            #region JWT Authentication

            var section = configuration.GetSection("jwtSettings");
            if (!section.Exists())
            {
                throw new Exception("jwtSettings section is missing in appsettings.json");
            }

            var jwtSettings = section.Get<JwtSettings>();
            services.AddSingleton(jwtSettings);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = jwtSettings.ValidateIssuer,
                   ValidIssuers = new[] { jwtSettings.Issuer },
                   ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                   ValidAudience = jwtSettings.Audience,
                   ValidateAudience = jwtSettings.ValidateAudience,
                   ValidateLifetime = jwtSettings.ValidateLifeTime,
               };
           });
            #endregion

            #region Swagger Configurations

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("All", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Book Store Managment System",
                    Description = "All Endpoints",

                });
                c.SwaggerDoc("Accounts", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Accounts",
                    Description = "Accounts Web Api in Asp.net Core Version .net7",

                });
                c.SwaggerDoc("Admins", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Admins",
                    Description = "Admins Web Api in Asp.net Core Version .net8",

                });
                c.SwaggerDoc("Authors", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Authors",
                    Description = "Authors Web Api in Asp.net Core Version .net7",

                });
                c.SwaggerDoc("Books", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Books",
                    Description = "Books Web Api in Asp.net Core Version .net7",

                });
                c.SwaggerDoc("Catlogs", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Catlogs",
                    Description = "Catlogs Web Api in Asp.net Core Version .net7",

                });
                c.SwaggerDoc("Customers", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Customers",
                    Description = "Customers Web Api in Asp.net Core Version .net7",

                });
                c.SwaggerDoc("Orders", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Orders",
                    Description = "Orders Web Api in Asp.net Core Version .net7",

                });
                c.SwaggerDoc("OrderDetails", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OrderDetails",
                    Description = "OrderDetails Web Api in Asp.net Core Version .net7",

                });

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var groupName = apiDesc.GroupName;
                    if (docName == "All")
                        return true;
                    return docName == groupName;
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                         {
                                            {
                                                new OpenApiSecurityScheme
                                                {
                                                    Reference = new OpenApiReference
                                                    {
                                                        Type = ReferenceType.SecurityScheme,
                                                        Id = JwtBearerDefaults.AuthenticationScheme
                                                    }
                                                },
                                                Array.Empty<string>()
                                            }
                                        });
                c.EnableAnnotations();
            });

            #endregion

            return services;
        }
    }
}