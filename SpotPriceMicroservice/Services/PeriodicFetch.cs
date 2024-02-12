using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using SpotPriceMicroservice.Services;
namespace SpotPriceMicroservice.Services
{
    public class PeriodicFetch :IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly TimeSpan _fetchInterval = TimeSpan.FromHours(1);
        private readonly FetchData _fetchData;
        public PeriodicFetch(FetchData fetchData)
        {
            _fetchData = fetchData;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("PeriodicFetch started at " + DateTime.Now);
            _timer = new Timer(state => FetchDataCallBack(), null, TimeSpan.Zero, _fetchInterval);
            return Task.CompletedTask;
        }

        public async Task FetchDataCallBack()
        {
            try
            {
                LatestPriceData latestPriceData = await _fetchData.FetchLatestPriceData();

                //ForwardDataToMicroservice(latestPriceData).Wait();
                Console.WriteLine("Data fetched successfully at " + DateTime.Now);
                Console.WriteLine("Fetched data: " + JsonConvert.SerializeObject(latestPriceData));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching data: {ex.Message}");
            }
        }
        public Task StopAsync(CancellationToken cancelToken)
        {
            Console.WriteLine("PeriodicFetch stopped at " + DateTime.Now);
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
