using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StoreMarket.Abstractions;
using StoreMarket.Contexts;
using StoreMarket.Mappers;
using StoreMarket.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        builder.Services.AddDbContext<StoreContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddMemoryCache(m => m.TrackStatistics = true);
        builder.Services.AddScoped<IProductServices, ProductServices>();

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        //builder.Services.AddMemoryCache(m=>m.TrackStatistics = true); // удалите эту строку

        // Перенесите вызов AddDbContext перед созданием WebApplication
        builder.Services.AddDbContext<StoreContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Server=(localdb)\\mssqllocaldb;Database=StoreDB;TrustServerCertificate=True;")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

