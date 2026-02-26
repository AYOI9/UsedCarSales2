using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using UsedCarsClient.Models;

namespace UsedCarsClient.Services
{
    public class ClientsService : BaseService<Client>
    {
        private readonly HttpClient httpClient;

        public ClientsService(string token)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public override async Task Add(Client obj)
        {
            JsonContent content = JsonContent.Create(obj);
            using var response = await httpClient.PostAsync("https://localhost:7229/api/Clients", content);
            response.EnsureSuccessStatusCode();
        }

        public override async Task Delete(Client obj)
        {
            using var response = await httpClient.DeleteAsync($"https://localhost:7229/api/Clients/{obj.ClientId}");
            response.EnsureSuccessStatusCode();
        }

        public override async Task<List<Client>> GetAll()
        {
            var list = await httpClient.GetFromJsonAsync<List<Client>>("https://localhost:7229/api/Clients");
            return list ?? new List<Client>();
        }

        public override Task<List<Client>> Search(string str)
        {
            // при желании можно реализовать поиск на клиенте
            return GetAll();
        }

        public override async Task Update(Client obj)
        {
            JsonContent content = JsonContent.Create(obj);
            using var response = await httpClient.PutAsync($"https://localhost:7229/api/Clients/{obj.ClientId}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
