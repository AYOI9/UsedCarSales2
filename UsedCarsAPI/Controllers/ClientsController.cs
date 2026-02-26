using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedCarsAPI.Models;
using UsedCarsAPI.Services;

namespace UsedCarsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsService clientService;

        public ClientsController(ClientsService service)
        {
            clientService = service;
        }

        // список всех клиентов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await clientService.GetAll();
            return Ok(clients);
        }

        // поиск одного клиента по коду
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await clientService.GetById(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        // добавление нового клиента
        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient([FromBody] Client client)
        {
            await clientService.Create(client);
            return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, client);
        }

        // изменение данных клиента
        [HttpPut("{id}")]
        public async Task<ActionResult<Client>> UpdateClient(int id, [FromBody] Client client)
        {
            if (client.ClientId != id) return BadRequest();
            await clientService.Update(client);
            return Ok(client);
        }

        // удаление клиента
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await clientService.Delete(id);
            return NoContent();
        }
    }
}
