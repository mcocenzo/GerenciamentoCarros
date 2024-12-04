using GerenciamentoDeCarrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoDeCarrosAPI.Controllers
{
    [ApiController]
    [Route("api/pessoas/{idPessoa}/favoritos")]
    public class CarroFavoritoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarroFavoritoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/pessoas/{idPessoa}/favoritos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carro>>> GetFavoritos(int idPessoa)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.CarrosFavoritos)
                .ThenInclude(cf => cf.Carro)
                .FirstOrDefaultAsync(p => p.Id == idPessoa);

            if (pessoa == null)
                return NotFound($"Pessoa com ID {idPessoa} não encontrada.");

            return Ok(pessoa.CarrosFavoritos.Select(cf => cf.Carro));
        }

        // POST: api/pessoas/{idPessoa}/favoritos/{idCarro}
        [HttpPost("{idCarro}")]
        public async Task<IActionResult> AddFavorito(int idPessoa, int idCarro)
        {
            var pessoa = await _context.Pessoas.FindAsync(idPessoa);
            if (pessoa == null)
                return NotFound($"Pessoa com ID {idPessoa} não encontrada.");

            var carro = await _context.Carros.FindAsync(idCarro);
            if (carro == null)
                return NotFound($"Carro com ID {idCarro} não encontrado.");

            // Verificar se já existe o favorito
            var favoritoExistente = await _context.CarrosFavoritos
                .FirstOrDefaultAsync(cf => cf.PessoaId == idPessoa && cf.CarroId == idCarro);

            if (favoritoExistente != null)
                return BadRequest("O carro já está nos favoritos dessa pessoa.");

            // Adicionar o favorito
            var favorito = new CarroFavorito
            {
                PessoaId = idPessoa,
                CarroId = idCarro
            };

            _context.CarrosFavoritos.Add(favorito);
            await _context.SaveChangesAsync();

            return Ok("Carro adicionado aos favoritos.");
        }

        // DELETE: api/pessoas/{idPessoa}/favoritos/{idCarro}
        [HttpDelete("{idCarro}")]
        public async Task<IActionResult> RemoveFavorito(int idPessoa, int idCarro)
        {
            var favorito = await _context.CarrosFavoritos
                .FirstOrDefaultAsync(cf => cf.PessoaId == idPessoa && cf.CarroId == idCarro);

            if (favorito == null)
                return NotFound("Favorito não encontrado.");

            _context.CarrosFavoritos.Remove(favorito);
            await _context.SaveChangesAsync();

            return Ok("Carro removido dos favoritos.");
        }
    }
}
