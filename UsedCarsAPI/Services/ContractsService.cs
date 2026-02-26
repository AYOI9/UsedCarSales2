using Microsoft.EntityFrameworkCore;
using UsedCarsAPI.Models;

namespace UsedCarsAPI.Services
{
    public class ContractsService : IService<Contract>
    {
        private readonly UsedCarsDb16Context db;

        public ContractsService(UsedCarsDb16Context context)
        {
            db = context;
        }

        public async Task<IEnumerable<Contract>> GetAll()
        {
            return await db.Contracts.ToListAsync();
        }

        public async Task<Contract> GetById(int id)
        {
            return await db.Contracts.FirstOrDefaultAsync(c => c.ContractId == id);
        }

        public async Task Create(Contract entity)
        {
            db.Contracts.Add(entity);
            await db.SaveChangesAsync();
        }

        public async Task Update(Contract entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.Contracts.Update(entity);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var contract = await db.Contracts.FindAsync(id);
            if (contract != null)
            {
                db.Contracts.Remove(contract);
                await db.SaveChangesAsync();
            }
        }

        // запрос: количество договоров по каждому клиенту
        public async Task<IEnumerable<object>> GetContractsCountByClient()
        {
            return await db.Contracts
                .GroupBy(c => c.ClientId)
                .Select(g => new { ClientId = g.Key, ContractsCount = g.Count() })
                .Join(db.Clients,
                    g => g.ClientId,
                    c => c.ClientId,
                    (g, c) => new
                    {
                        ClientId = c.ClientId,
                        ClientName = c.LastName + " " + c.FirstName,
                        ContractsCount = g.ContractsCount
                    })
                .ToListAsync();
        }

        // запрос: договоры за период
        public async Task<IEnumerable<Contract>> GetByPeriod(DateTime dateFrom, DateTime dateTo)
        {
            return await db.Contracts
                .Where(c => c.ContractDate >= dateFrom && c.ContractDate <= dateTo)
                .ToListAsync();
        }
    }
}
