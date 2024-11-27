using BookStore.Application;
using BookStore.Infrastructure.Context;
using BookStore.Infrastucture;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connection To SQL Server
builder.Services.AddDbContext<BookDBContext>(option =>
{
    option.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Book_CON"));
});

#region Dependency injections

builder.Services.AddInfrastructureDependencies()
                .AddApplicationDependencies()
                .AddServiceRegisteration(builder.Configuration);

#endregion

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
        options.SwaggerEndpoint("/swagger/OrderDetails/swagger.json", "OrderDetails");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
