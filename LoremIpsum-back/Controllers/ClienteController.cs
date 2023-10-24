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
            if (!IsValidSexo(cliente.Sexo))
            {
                return BadRequest("Sexo deve ser 'M' ou 'F'.");
            }

            dbcontext.Cliente.Add(cliente);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction("PostCliente", new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
        {
            if (!IsValidSexo(cliente.Sexo))
            {
                return BadRequest("Sexo deve ser 'M' ou 'F'.");
            }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await dbcontext.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            
            var enderecos = await dbcontext.Endereco.Where(e => e.IdCliente == id).ToListAsync();
            dbcontext.Endereco.RemoveRange(enderecos);
            
            dbcontext.Cliente.Remove(cliente);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return dbcontext.Cliente.Any(e => e.Id == id);
        }

        private bool IsValidSexo(char sexo)
        {
            return sexo == 'M' || sexo == 'F';
        }
    }
}
