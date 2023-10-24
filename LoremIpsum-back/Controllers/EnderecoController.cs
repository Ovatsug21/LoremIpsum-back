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
            return await dbcontext.Endereco.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Endereco>> GetEnderecoId(int id)
        {
            var endereco = await dbcontext.Endereco.FindAsync(id);

            if (endereco == null)
            {
                return NotFound();
            }

            return endereco;
        }

        [HttpPost]
        public async Task<ActionResult<Endereco>> PostEndereco(Endereco endereco)
        {
            dbcontext.Endereco.Add(endereco);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction("PostEndereco", new { id = endereco.Id }, endereco);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(int id, Endereco endereco)
        {
            if (id != endereco.Id)
            {
                return BadRequest();
            }

            dbcontext.Entry(endereco).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnderecoExists(id))
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
