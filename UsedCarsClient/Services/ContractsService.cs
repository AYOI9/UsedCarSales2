using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UsedCarsClient.Models;

namespace UsedCarsClient.Services
{
    public class ContractsService : BaseService<Contract>
    {
        private readonly HttpClient httpClient;

        public ContractsService(string token)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public override async Task<List<Contract>> GetAll()
        {
            var list = await httpClient.GetFromJsonAsync<List<Contract>>("https://localhost:7229/api/Contracts");
            return list ?? new List<Contract>();
        }

        public override async Task Add(Contract obj)
        {
            JsonContent content = JsonContent.Create(obj);
            using var response = await httpClient.PostAsync("https://localhost:7229/api/Contracts", content);
            response.EnsureSuccessStatusCode();
        }

        public override async Task Update(Contract obj)
        {
            JsonContent content = JsonContent.Create(obj);
            using var response = await httpClient.PutAsync($"https://localhost:7229/api/Contracts/{obj.ContractId}", content);
            response.EnsureSuccessStatusCode();
        }

        public override async Task Delete(Contract obj)
        {
            using var response = await httpClient.DeleteAsync($"https://localhost:7229/api/Contracts/{obj.ContractId}");
            response.EnsureSuccessStatusCode();
        }

        public override Task<List<Contract>> Search(string str)
        {
            // при желании можно реализовать поиск на клиенте
            return GetAll();
        }
    }
}
