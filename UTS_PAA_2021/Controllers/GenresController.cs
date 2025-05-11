using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTS_PAA_2021.Data;
using UTS_PAA_2021.Models;

namespace UTS_PAA_2021.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GenresController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres() =>
            await _context.Genres.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            return genre == null ? NotFound() : Ok(genre);
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> CreateGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, Genre genreFromBody)
        {
            if (string.IsNullOrEmpty(genreFromBody.Name))
            {
                return BadRequest("Nama genre tidak boleh kosong.");
            }

            var existingGenre = await _context.Genres.FindAsync(id);
            if (existingGenre == null)
            {
                return NotFound();
            }

            existingGenre.Name = genreFromBody.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return NotFound();

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

