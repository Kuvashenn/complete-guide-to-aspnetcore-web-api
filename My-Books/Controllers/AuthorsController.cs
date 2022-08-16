using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Books.Data.Services;
using My_Books.Data.ViewModels;

namespace My_Books.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthorsController : ControllerBase
  {
    private readonly AuthorsService _authorsService;

    public AuthorsController(AuthorsService authorsService)
    {
      _authorsService = authorsService;
    }

    [HttpPost("add-author")]
    public IActionResult AddAuthor([FromBody] AuthorVM author)
    {
      _authorsService.AddAuthor(author);
      return Ok();
    }

    [HttpGet("get-author-with-books-by-id/{id}")]
    public IActionResult GetAuthorWithBooks(int id)
    {
      return Ok(_authorsService.GetAuthorWithBooks(id));
    }
  }
}
