using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime;

namespace SpotPriceMicroservice.Services
{
    public class FetchData
    {
        private readonly HttpClient _priceApiClient;
        private readonly HttpClient _databaseApiClient;

        public FetchData(HttpClient priceApiClient, HttpClient databaseApiClient)
        {
            _priceApiClient = priceApiClient;
            _databaseApiClient = databaseApiClient;
        }
        
        public async Task<LatestPriceData> FetchLatestPriceData()
        {
            try
            {
                HttpResponseMessage response = await _priceApiClient.GetAsync(Constants.SpotPriceApiUrl);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                LatestPriceData latestPriceData = JsonConvert.DeserializeObject<LatestPriceData>(json);

                await StoreDataInDatabase(latestPriceData);

                //if (latestPriceData == null)
                //{
                //    Console.Error.WriteLine("Deserialization failed. Received JSON: " + json);
                //}

                return latestPriceData;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        private async Task StoreDataInDatabase(LatestPriceData latestPriceData)
        {
            try
            {
                string endpoint = "api/Prices";
                HttpResponseMessage response = await _databaseApiClient.PostAsJsonAsync(endpoint, latestPriceData);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error storing data in the database: {ex.Message}");
            }
        }
    }
}
