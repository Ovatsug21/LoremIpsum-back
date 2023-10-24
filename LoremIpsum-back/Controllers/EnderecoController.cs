using LoremIpsum_back.Context;
using LoremIpsum_back.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoremIpsum_back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly AppDbContext dbcontext;

        public EnderecoController(AppDbContext context)
        {
            dbcontext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Endereco>>> GetEnderecos()
        {
            var enderecos = await dbcontext.Endereco.Include(e => e.Cliente).ToListAsync();
            return enderecos;
        }

        //Dando conflito
        /*[HttpGet("{id}")]
        public async Task<ActionResult<Endereco>> GetEnderecoIdCliente(int id)
        {
            var endereco = await dbcontext.Endereco.Include(e => e.Cliente)
            .FirstOrDefaultAsync(e => e.Id == id);

            if (endereco == null)
            {
                return NotFound();
            }

            return endereco;
        }*/

        [HttpGet("{idCliente}")]
        public async Task<ActionResult<IEnumerable<Endereco>>> GetEnderecosCliente(int idCliente)
        {
            var enderecos = await dbcontext.Endereco.Include(e => e.Cliente)
                .Where(e => e.IdCliente == idCliente)
                .ToListAsync();

            return enderecos;
        }

        [HttpPost]
        public async Task<ActionResult<Endereco>> PostEndereco(int idCliente, Endereco endereco)
        {
            var clienteExistente = await dbcontext.Cliente.FindAsync(idCliente);
            if (clienteExistente == null)
            {
                return NotFound();
            }

            endereco.IdCliente = idCliente;
            endereco.Cliente = clienteExistente;

            dbcontext.Endereco.Add(endereco);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction("PostEndereco", new { id = endereco.Id }, endereco);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEndereco(int id, Endereco endereco)
        {
            if (id != endereco.Id)
            {
                return BadRequest("O ID do endereço na URL deve corresponder ao ID do endereço no corpo da solicitação.");
            }

            //Assegura que IdCliente não seja alterado durante o update
            var confereEndereco = await dbcontext.Endereco.FindAsync(id);
            if (confereEndereco == null)
            {
                return NotFound("Endereço não encontrado.");
            }

            endereco.IdCliente = confereEndereco.IdCliente;

            dbcontext.Entry(confereEndereco).CurrentValues.SetValues(endereco);

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnderecoExists(id))
                {
                    return NotFound("Endereço não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool EnderecoExists(int id)
        {
            return dbcontext.Endereco.Any(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEndereco(int id)
        {
            var endereco = await dbcontext.Endereco.FindAsync(id);
            if (endereco == null)
            {
                return NotFound();
            }

            dbcontext.Endereco.Remove(endereco);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

    }
}
