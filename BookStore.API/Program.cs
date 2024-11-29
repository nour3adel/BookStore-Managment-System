using BookStore.Application;
using BookStore.Infrastructure.Context;
using BookStore.Infrastucture;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://yourclientdomain.com") // Add allowed origins
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow credentials if needed
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




#region SQL SERVER Connection

builder.Services.AddDbContext<BookDBContext>(option =>
{
    option.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Book_CON"));
});

#endregion

#region Dependency injections

builder.Services.AddInfrastructureDependencies()
                .AddApplicationDependencies()
                .AddServiceRegisteration(builder.Configuration);

#endregion

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    #region Swagger

    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/All/swagger.json", "All");
        options.SwaggerEndpoint("/swagger/Accounts/swagger.json", "Accounts");
        options.SwaggerEndpoint("/swagger/Admins/swagger.json", "Admins");
        options.SwaggerEndpoint("/swagger/Authors/swagger.json", "Authors");
        options.SwaggerEndpoint("/swagger/Books/swagger.json", "Books");
        options.SwaggerEndpoint("/swagger/Catlogs/swagger.json", "Catlogs");
        options.SwaggerEndpoint("/swagger/Customers/swagger.json", "Customers");
        options.SwaggerEndpoint("/swagger/Orders/swagger.json", "Orders");

        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.DefaultModelsExpandDepth(-1);
    });

    #endregion
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
