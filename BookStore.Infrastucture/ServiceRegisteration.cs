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
                //// Password settings.
                //option.Password.RequireDigit = true;
                //option.Password.RequireLowercase = true;
                //option.Password.RequireNonAlphanumeric = true;
                //option.Password.RequireUppercase = true;
                //option.Password.RequiredLength = 6;
                //option.Password.RequiredUniqueChars = 1;

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
                    Title = "Book Store Management System",
                    Description = @"
                                 The BookStore API provides a RESTful interface for managing an online bookstore’s inventory,
                                customer orders, and author information. The API allows users to view available books,
                                manage inventory, handle customer orders, and retrieve author details.
                            ",
                    TermsOfService = new Uri("https://www.nouradel.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nour Adel",
                        Email = "nour3dell@gmail.com",
                        Url = new Uri("https://www.nouradel.com/contact")
                    }
                });
                c.SwaggerDoc("Accounts", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Accounts",


                    Description = @"
                                 The BookStore API provides a RESTful interface for managing an online bookstore’s inventory,
                                customer orders, and author information. The API allows users to view available books,
                                manage inventory, handle customer orders, and retrieve author details.
                            ",

                });
                c.SwaggerDoc("Admins", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Admins",

                    Description = @"
                                 This API provides functionalities that are accessible only to admin users. 
        Admins have the ability to manage the entire bookstore, including adding, 
        updating, and deleting books and authors, as well as handling orders. 
        Only users with admin privileges can perform certain critical operations 
        such as deleting or updating orders, books, and author information.
                            ",

                });
                c.SwaggerDoc("Authors", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Authors",

                    Description = @"
                                  This API allows users to manage authors in the bookstore. 
                                You can view a list of authors, add new authors, 
                                update author details, and delete authors (admin access only).
                            ",

                });
                c.SwaggerDoc("Books", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Books",

                    Description = @"
                                 This API allows users to manage books, authors, and customer orders for an online bookstore. 
        It provides functionalities for viewing books, adding, updating, and deleting books (admin only), 
        and managing customer orders. The API is designed to streamline bookstore inventory and order management.
                            ",

                });
                c.SwaggerDoc("Catlogs", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Catlogs",

                    Description = @"
                                 The BookStore API provides a RESTful interface for managing an online bookstore’s inventory,
                                customer orders, and author information. The API allows users to view available books,
                                manage inventory, handle customer orders, and retrieve author details.
                            ",

                });
                c.SwaggerDoc("Customers", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Customers",

                    Description = @"
                                This API allows users to manage customer orders in the bookstore. 
        You can fetch a list of all orders, view specific order details, 
        place new orders, update order status, and delete orders (admin access only).
                            ",

                });
                c.SwaggerDoc("Orders", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Orders",

                    Description = @"
                                  This API allows users to manage customer orders within the bookstore. 
        It provides functionality for creating new orders, updating existing orders, 
        fetching the details of specific orders, and deleting orders. 
        Admins have special access to delete or update any orders, while customers 
        can only manage their own orders.
                            ",

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