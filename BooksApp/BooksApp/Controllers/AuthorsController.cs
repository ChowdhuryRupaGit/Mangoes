using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksApp.Data;
using AutoMapper;
using BooksApp.Model.Author;

namespace BooksApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookAppStoreContext _context;
        private IMapper _mapper;
        private ILogger<AuthorsController> _logger;

        public AuthorsController(BookAppStoreContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            try
            {
                var author = await _context.Authors.ToListAsync();
                var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(author);
                return Ok(authorDtos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(GetAuthors)}");
                return NotFound();
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                var authorDto = _mapper.Map<AuthorDto>(author);
                return authorDto;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(GetAuthors)}");
                return NotFound();
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDto authorDto)
        {
            try
            {
                var author = _mapper.Map<Author>(authorDto);
                _context.Entry(author).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(PutAuthor)}");
                return BadRequest();
            }
            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(AuthorDto authorDto)
        {
            try
            {
                var author = _mapper.Map<Author>(authorDto);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
            }
            catch(Exception ex)
            {
                return Problem("Entity set 'BookAppStoreContext.Authors'  is null.");
            }
           
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(GetAuthors)}");
                return NotFound();
            }
            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
