
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreMarket.Abstractions;
using StoreMarket.Contexts;
using StoreMarket.Mappers;
using StoreMarket.Services;

namespace StoreMarket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<StoreContext>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(c =>
                                            c.RegisterType<ProductServices>()
                                            .As<IProductServices>());

            builder.Services.AddMemoryCache(m=>m.TrackStatistics = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            builder.Services.AddDbContext<StoreContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StoreDB;TrustServerCertificate=True;");


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
