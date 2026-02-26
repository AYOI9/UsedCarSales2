using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedCarsAPI.Models;
using UsedCarsAPI.Services;

namespace UsedCarsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly ContractsService contractService;

        public ContractsController(ContractsService service)
        {
            contractService = service;
        }

        // список договоров
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetAllContracts()
        {
            var contracts = await contractService.GetAll();
            return Ok(contracts);
        }

        // один договор по коду
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContractById(int id)
        {
            var contract = await contractService.GetById(id);
            if (contract == null) return NotFound();
            return Ok(contract);
        }

        // добавление договора
        [HttpPost]
        public async Task<ActionResult<Contract>> CreateContract([FromBody] Contract contract)
        {
            await contractService.Create(contract);
            return CreatedAtAction(nameof(GetContractById), new { id = contract.ContractId }, contract);
        }

        // изменение договора
        [HttpPut("{id}")]
        public async Task<ActionResult<Contract>> UpdateContract(int id, [FromBody] Contract contract)
        {
            if (contract.ContractId != id) return BadRequest();
            await contractService.Update(contract);
            return Ok(contract);
        }

        // удаление договора
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await contractService.Delete(id);
            return NoContent();
        }

        // количество договоров по каждому клиенту
        [HttpGet("per-client")]
        public async Task<ActionResult> GetContractsCountByClient()
        {
            var result = await contractService.GetContractsCountByClient();
            return Ok(result);
        }

        // договоры за заданный период
        [HttpGet("by-period")]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContractsByPeriod(DateTime dateFrom, DateTime dateTo)
        {
            var result = await contractService.GetByPeriod(dateFrom, dateTo);
            return Ok(result);
        }
    }
}
