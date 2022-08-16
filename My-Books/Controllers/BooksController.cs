using Microsoft.AspNetCore.Mvc;
using My_Books.Data.Services;
using My_Books.Data.ViewModels;

namespace My_Books.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BooksController : ControllerBase
  {
    private readonly BooksService _booksService;

    public BooksController(BooksService booksService)
    {
      _booksService = booksService;
    }

    [HttpPost("add-book-with-authors")]
    public IActionResult AddBook([FromBody] BookVM book)
    {
      _booksService.AddBookWithAuthors(book);
      return Ok();
    }

    [HttpGet("get-all-books")]
    public IActionResult GetAllBooks()
    {
      var allBooks = _booksService.GetAllBooks();
      return Ok(allBooks);
    }

    [HttpGet("get-book-by-id/{id}")]
    public IActionResult GetBookById(int id)
    {
      var book = _booksService.GetBook(id);
      return Ok(book);
    }

    [HttpPut("update-book-by-id/{id}")]
    public IActionResult UpdateBookById(int id, [FromBody] BookVM book)
    {
      var updatedBook = _booksService.UpdateBookById(id, book);
      return Ok(updatedBook);
    }

    [HttpDelete("delete-book-by-id/{id}")]
    public IActionResult DeleteBookById(int id)
    {
      _booksService.DeleteBookById(id);
      return Ok();
    }
  }
}
