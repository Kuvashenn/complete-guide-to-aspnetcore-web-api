using System.Linq;
using My_Books.Data.Models;
using My_Books.Data.ViewModels;

namespace My_Books.Data.Services
{
  public class AuthorsService
  {
    private readonly AppDbContext _context;

    public AuthorsService(AppDbContext context)
    {
      _context = context;
    }

    public void AddAuthor(AuthorVM book)
    {
      var _author = new Author
      {
        FullName = book.Fullname
      };
      _context.Authors.Add(_author);
      _context.SaveChanges();
    }

    public AuthoWithBooksVM GetAuthorWithBooks(int authorId)
    {
      var _author = _context.Authors.Where(n => n.Id == authorId).Select(n => new AuthoWithBooksVM()
      {
        FullName = n.FullName,
        BookTitles = n.Book_Authors.Select(n => n.Book.Title).ToList(),
      }).FirstOrDefault();

      return _author;
    }
  }
}