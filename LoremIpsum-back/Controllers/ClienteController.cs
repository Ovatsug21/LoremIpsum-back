using LoremIpsum_back.Context;
using LoremIpsum_back.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoremIpsum_back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext dbcontext;

        public ClienteController(AppDbContext context)
        {
            dbcontext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await dbcontext.Cliente.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetClienteId(int id)
        {
            var cliente = await dbcontext.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            dbcontext.Cliente.Add(cliente);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction("PostCliente", new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            dbcontext.Entry(cliente).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return dbcontext.Cliente.Any(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await dbcontext.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            dbcontext.Cliente.Remove(cliente);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
