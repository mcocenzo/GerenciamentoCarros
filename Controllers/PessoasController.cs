using GerenciamentoDeCarrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoDeCarrosAPI.Controllers
{
    [ApiController]
    [Route("api/pessoas")]
    public class PessoasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/pessoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            return await _context.Pessoas.ToListAsync();
        }

        // GET: api/pessoas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
                return NotFound($"Pessoa com ID {id} não encontrada.");

            return pessoa;
        }

        // POST: api/pessoas
        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            // Validação para garantir que o email seja único
            var emailExistente = await _context.Pessoas.AnyAsync(p => p.Email == pessoa.Email);
            if (emailExistente)
                return BadRequest("O email fornecido já está em uso.");

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, pessoa);
        }

        // PUT: api/pessoas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
                return BadRequest("ID informado não corresponde ao ID da pessoa.");

            var pessoaExistente = await _context.Pessoas.FindAsync(id);
            if (pessoaExistente == null)
                return NotFound($"Pessoa com ID {id} não encontrada.");

            // Atualizar apenas os campos relevantes
            pessoaExistente.Nome = pessoa.Nome;
            pessoaExistente.Email = pessoa.Email;

            _context.Entry(pessoaExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/pessoas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
                return NotFound($"Pessoa com ID {id} não encontrada.");

            // Remover também os favoritos associados
            var favoritos = _context.CarrosFavoritos.Where(cf => cf.PessoaId == id);
            _context.CarrosFavoritos.RemoveRange(favoritos);

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return Ok($"Pessoa com ID {id} foi removida.");
        }
    }
}