using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using My_Books.Data;
using My_Books.Data.Models;
using My_Books.Data.Services;
using My_Books.Data.ViewModels;
using My_Books.Exceptions;
using NUnit.Framework;

namespace my_books_tests
{
  public class PublishersServiceTest
  {
    // need to come order the tests
    private static readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: "BookDbTest")
      .Options;

    private AppDbContext _context; 
    PublishersService _publishersService;

    [OneTimeSetUp]
    public void Setup()
    {
      _context = new AppDbContext(_dbContextOptions);
      _context.Database.EnsureCreated();

      SeedDatabase();

      _publishersService = new PublishersService(_context);

    }

    private void SeedDatabase()
    {
      var publishers = new List<Publisher>
            {
                    new Publisher() {
                        Id = 1,
                        Name = "Publisher Test1"
                    },
                    new Publisher() {
                        Id = 2,
                        Name = "Publisher Test2"
                    },
                    new Publisher() {
                        Id = 3,
                        Name = "Publisher Test3"
                    },
                    new Publisher() {
                        Id = 4,
                        Name = "Publisher Test4"
                    },
                    new Publisher() {
                        Id = 5,
                        Name = "Publisher Test5"
                    },
                    new Publisher() {
                        Id = 6,
                        Name = "Publisher Test6"
                    }
            };
      _context.Publishers.AddRange(publishers);

      var authors = new List<Author>()
            {
                new Author()
                {
                    Id = 1,
                    FullName = "Author 1"
                },
                new Author()
                {
                    Id = 2,
                    FullName = "Author 2"
                }
            };
      _context.Authors.AddRange(authors);


      var books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title = "Book 1 Title",
                    Description = "Book 1 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                },
                new Book()
                {
                    Id = 2,
                    Title = "Book 2 Title",
                    Description = "Book 2 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                }
            };
      _context.Books.AddRange(books);

      var books_authors = new List<Book_Author>()
            {
                new Book_Author()
                {
                    Id = 1,
                    BookId = 1,
                    AuthorId = 1
                },
                new Book_Author()
                {
                    Id = 2,
                    BookId = 1,
                    AuthorId = 2
                },
                new Book_Author()
                {
                    Id = 3,
                    BookId = 2,
                    AuthorId = 2
                },
            };
      _context.Book_Authors.AddRange(books_authors);


      _context.SaveChanges();
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
      _context.Database.EnsureDeleted();
    }

    [Test, Order(9)]
    public void GetPublisherData_Test()
    {
      var result = _publishersService.GetPublisherData(1);

      Assert.That(result.Name, Is.EqualTo("Publisher Test1"));
      Assert.That(result.BookAuthors, Is.Not.Empty);
      Assert.That(result.BookAuthors.Count, Is.GreaterThan(0));

      Assert.That(result.BookAuthors.OrderBy(n => n.BookName).FirstOrDefault().BookName, Is.EqualTo("Book 1 Title"));
    }

    [Test, Order(8)]
    public void AddPublisher_WithException()
    {
      var newPublisher = new PublisherVM() { Name = "123 with exception" };

      Assert.That(() => _publishersService.AddPublisher(newPublisher), Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo("Name starts with number"));
    }

    [Test, Order(7)]
    public void AddPublisher_WithOutException()
    {
      var newPublisher = new PublisherVM() { Name = "Without Exception" };

      var result = _publishersService.AddPublisher(newPublisher);

      Assert.That(result, Is.Not.Null);
      Assert.That(result.Name, Does.StartWith("Without"));
      Assert.That(result.Id, Is.Not.Null);
    }

    [Test, Order(6)]
    public void GetPublisherById_WithResponse_Test()
    {
      var result = _publishersService.GetPublisherById(1);
      Assert.That(result.Id, Is.EqualTo(1));
      Assert.That(result.Name, Is.EqualTo("Publisher Test1"));
    }

    [Test, Order(5)]
    public void GetPublisherById_WithOutResponse_Test()
    {
      var result = _publishersService.GetPublisherById(99);
      Assert.That(result, Is.Null);
    }

    [Test, Order(1)]
    public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithNoPageNumber()
    {
      // Don't forget the page size here that limits the results.
      var result = _publishersService.GetAllPublishers("", "", null);
      Assert.That(result.Count, Is.EqualTo(5));
      Assert.AreEqual(result.Count, 5);
    }

    [Test, Order(2)]
    public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithPageNumber()
    {
      // Don't forget the page size here that limits the results.
      var result = _publishersService.GetAllPublishers("", "", 2);
      Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test, Order(3)]
    public void GetAllPublishers_WithNoSortBy_WithSearchString_WithNoPageNumber()
    {
      // Don't forget the page size here that limits the results.
      var result = _publishersService.GetAllPublishers("", "3", null);
      Assert.That(result.Count, Is.EqualTo(1));
      Assert.That(result.FirstOrDefault()?.Name, Is.EqualTo("Publisher Test3"));
    }

    [Test, Order(4)]
    public void GetAllPublishers_WithSortBy_WithNoSearchString_WithNoPageNumber()
    {
      // Don't forget the page size here that limits the results.
      var result = _publishersService.GetAllPublishers("name_desc", "", null);
      Assert.That(result.Count, Is.EqualTo(5));
      Assert.That(result.FirstOrDefault()?.Name, Is.EqualTo("Publisher Test6"));
    }

  }
}