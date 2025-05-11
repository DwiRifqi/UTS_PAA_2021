using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTS_PAA_2021.Data;
using UTS_PAA_2021.Models;

namespace UTS_PAA_2021.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            return await _context.Films.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == id);

            if (film == null)
                return NotFound();

            return Ok(film);
        }

        [HttpPost]
        public async Task<ActionResult<Film>> CreateFilm(Film film)
        {
            film.Id = 0;

            _context.Films.Add(film);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFilm(int id, Film filmFromBody)
        {
            if (string.IsNullOrEmpty(filmFromBody.Title) || string.IsNullOrEmpty(filmFromBody.Description))
            {
                return BadRequest("Judul dan Deskripsi tidak boleh kosong.");
            }

            var existingFilm = await _context.Films.FindAsync(id);
            if (existingFilm == null)
                return NotFound();

            existingFilm.Title = filmFromBody.Title;
            existingFilm.Description = filmFromBody.Description;

            await _context.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
                return NotFound();

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
