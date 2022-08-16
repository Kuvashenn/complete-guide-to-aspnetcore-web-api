using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using My_Books.Data.Models;
using My_Books.Data.ViewModels;

namespace My_Books.Data.Services
{
  public class BooksService
  {
    private readonly AppDbContext _context;

    public BooksService(AppDbContext context)
    {
      _context = context;
    }

    public void AddBookWithAuthors(BookVM book)
    {
      var _book = new Book()
      {
        Title = book.Title,
        Description = book.Description,
        IsRead = book.IsRead,
        DateRead = book.IsRead ? book.DateRead : null,
        Rate = book.IsRead ? book.Rate : null,
        Genre = book.Genre,
        CoverUrl = book.CoverUrl,
        DateAdded = DateTime.Now,
        PublisherId = book.PublisherId
      };
      _context.Books.Add(_book);
      _context.SaveChanges();

      foreach (var id in book.AuthorIds)
      {
        var _book_author = new Book_Author
        {
          BookId = _book.Id,
          AuthorId = id
        };
        _context.Book_Authors.Add(_book_author);
        _context.SaveChanges();
      }
    }

    public List<Book> GetAllBooks()
    {
      return _context.Books.ToList();
    }

    public BookWithAuthorsVM GetBook(int bookId)
    {
      var _bookWithAuthors = _context.Books.Where(n => n.Id == bookId).Select(book => new BookWithAuthorsVM()
      {
        Title = book.Title,
        Description = book.Description,
        IsRead = book.IsRead,
        DateRead = book.IsRead ? book.DateRead : null,
        Rate = book.IsRead ? book.Rate : null,
        Genre = book.Genre,
        CoverUrl = book.CoverUrl,
        PublisherName = book.Publisher.Name,
        AuthorsNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
      }).FirstOrDefault();
      return _bookWithAuthors;
    }

    public Book UpdateBookById(int bookId, BookVM book)
    {
      var _book = _context.Books.FirstOrDefault(b => b.Id == bookId);

      if (_book == null) return _book;
      _book.Title = book.Title;
      _book.Description = book.Description;
      _book.IsRead = book.IsRead;
      _book.DateRead = book.IsRead ? book.DateRead : null;
      _book.Rate = book.IsRead ? book.Rate : null;
      _book.Genre = book.Genre;
      _book.CoverUrl = book.CoverUrl;

      _context.SaveChanges();
      return _book;
    }

    public void DeleteBookById(int bookId)
    {
      var _book = _context.Books.FirstOrDefault(b => b.Id == bookId);

      if(_book == null) return;
      _context.Books.Remove(_book);
      _context.SaveChanges();
    }
  }
}