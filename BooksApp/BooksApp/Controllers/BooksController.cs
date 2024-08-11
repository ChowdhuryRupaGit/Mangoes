using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksApp.Data;
using AutoMapper;
using BooksApp.Model.Book;

namespace BooksApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookAppStoreContext _context;
        private IMapper _mapper;
        private ILogger<AuthorsController> _logger;

        public BooksController(BookAppStoreContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            try
            {
                var book = await _context.Books.ToListAsync();
                return Ok(_mapper.Map<BookDto>(book));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(GetBooks)}");
                return NotFound();
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                return _mapper.Map<BookDto>(book);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(GetBooks)}");
                return NotFound();
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookDto bookdto)
        {
            try
            {
                var book = _mapper.Map<Book>(bookdto);
                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(PutBook)}");
                return BadRequest();
            }
            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBook(BookDto bookdto)
        {
            try
            {
                var book =_mapper.Map<Book>(bookdto);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetBook", new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(PutBook)}");
                return BadRequest();
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while requesting {nameof(PutBook)}");
                return BadRequest();
            }
            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
