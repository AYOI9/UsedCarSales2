using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using UsedCarsClient.Models;

namespace UsedCarsClient.Services
{
    public class AuthService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<string?> Register(Person person)
        {
            JsonContent content = JsonContent.Create(person);
            using var response = await client.PostAsync("https://localhost:7229/register", content);
            if (!response.IsSuccessStatusCode) return null;
            return "OK";
        }

        public async Task<Response?> SignIn(Person person)
        {
            JsonContent content = JsonContent.Create(person);
            using var response = await client.PostAsync("https://localhost:7229/login", content);
            string responseText = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(responseText))
            {
                Response? resp = JsonSerializer.Deserialize<Response>(responseText);
                return resp;
            }
            return null;
        }
    }
}
