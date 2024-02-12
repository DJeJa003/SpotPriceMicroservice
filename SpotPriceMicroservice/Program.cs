
using SpotPriceMicroservice.Services;
using System.Security.Cryptography.X509Certificates;
using MicroserviceDB;
using System.Runtime;

namespace SpotPriceMicroservice
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();
            builder.Services.AddHostedService<PeriodicFetch>();
            builder.Services.AddSingleton<FetchData>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

    public class Prices
    {
        public int Id { get; set; }
        public decimal PriceValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class LatestPriceData
    {
        public List<PriceEntry> Prices { get; set; }
    }

    public class PriceEntry
    {
        public decimal Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
