using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpotPriceMicroservice.Services;
using System;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using System.Runtime;

namespace SpotPriceMicroservice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("GetSahko")]
        public async Task<IActionResult> GetLatestPrices()
        {
            try
            {
                var response = await _httpClient.GetAsync(Constants.SpotPriceApiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                //await SendDataToOtherService(content);

                return Ok(content);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, "Error fetching the data: " + e.Message);
            }
        }

        private async Task SendDataToOtherService(string content)
        {
            var url = Constants.DatabaseApiUrl;
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, stringContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Server response: {responseContent}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error sending request: {e.Message}");
            }
        }
    }
}
