using GerenciamentoCarros.DTOs;
using GerenciamentoDeCarrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GerenciamentoDeCarrosAPI.Controllers
{
    [ApiController]
    [Route("api/carros")]
    public class CarrosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carro>>> GetCarros()
        {
            return await _context.Carros.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Carro>> GetCarro(int id)
        {
            var carro = await _context.Carros.FindAsync(id);
            if (carro == null) return NotFound();
            return carro;
        }

        [HttpPost]
        public async Task<ActionResult<Carro>> PostCarro(CarroPostDto carroDto)
        {

            var carro = new Carro
            {
                Marca = carroDto.Marca,
                Modelo = carroDto.Modelo,
                Ano = carroDto.Ano,
                Preco = carroDto.Preco
            };

            if (carro.Ano < 1900 || carro.Preco < 0 || string.IsNullOrWhiteSpace(carro.Marca) || string.IsNullOrWhiteSpace(carro.Modelo))
                return BadRequest("Dados inválidos.");

            _context.Carros.Add(carro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCarro), new { id = carro.Id }, carro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarro(int id, CarroUpdateDto carroDto)
        {
            _context.Entry(carroDto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarro(int id)
        {
            var carro = await _context.Carros.FindAsync(id);
            if (carro == null) return NotFound();

            _context.Carros.Remove(carro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
