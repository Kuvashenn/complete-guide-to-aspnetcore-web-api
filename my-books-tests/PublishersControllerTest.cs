using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using My_Books.Controllers;
using My_Books.Data;
using My_Books.Data.Models;
using My_Books.Data.Services;
using My_Books.Data.ViewModels;
using NUnit.Framework;

namespace my_books_tests
{
  internal class PublishersControllerTest
  {
    private static readonly DbContextOptions<AppDbContext> _dbContextOptions =
      new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "BookDbTest")
        .Options;

    private AppDbContext _context;
    private PublishersService _publisherService;
    private PublishersController _publishersController;

    [OneTimeSetUp]
    public void Setup()
    {
      _context = new AppDbContext(_dbContextOptions);
      _context.Database.EnsureCreated();

      _publisherService = new PublishersService(_context);
      _publishersController = new PublishersController(_publisherService, new NullLogger<PublishersController>());

      SeedDatabase();
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

      _context.SaveChanges();
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
      _context.Database.EnsureDeleted();
    }

    [Test, Order(1)]
    public void GetAllPublishers_WithSortBy_WithSearchString_WithPageNumber_ReturnOk()
    {
      IActionResult actionResult = _publishersController.GetAllPublishers("name_desc", "Publisher", 1);
      Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
      var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;
      Assert.That(actionResultData.FirstOrDefault().Name, Is.EqualTo("Publisher Test6"));
      Assert.That(actionResultData.FirstOrDefault().Id, Is.EqualTo(6));
      Assert.That(actionResultData.Count, Is.EqualTo(5));

      IActionResult actionResultSecondPage = _publishersController.GetAllPublishers("name_desc", "Publisher", 2);
      Assert.That(actionResultSecondPage, Is.TypeOf<OkObjectResult>());
      var actionResultDataSecondPage = (actionResultSecondPage as OkObjectResult).Value as List<Publisher>;
      Assert.That(actionResultDataSecondPage.FirstOrDefault().Name, Is.EqualTo("Publisher Test1"));
      Assert.That(actionResultDataSecondPage.FirstOrDefault().Id, Is.EqualTo(1));
      Assert.That(actionResultDataSecondPage.Count, Is.EqualTo(1));

    }

    [Test, Order(2)]
    public void GetPublisherById_ReturnOk()
    {
      IActionResult actionResult = _publishersController.GetPublisherById(1);
      var actionResultData = (actionResult as OkObjectResult).Value as Publisher;
      
      Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
      Assert.That(actionResultData.Id, Is.EqualTo(1));
      Assert.That(actionResultData.Name, Is.EqualTo("publisher Test1").IgnoreCase);

    }

    [Test, Order(3)]
    public void GetPublisherById_ReturnNotFound()
    {
      IActionResult actionResult = _publishersController.GetPublisherById(10);

      Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
    }

    [Test, Order(4)]
    public void AddPublisher_ReturnsCreated()
    {
      var newPublishervM = new PublisherVM
      {
        Name = "New Publisher"
      };

      IActionResult actionResult = _publishersController.AddPublisher(newPublishervM);
      
      Assert.That(actionResult, Is.TypeOf<CreatedResult>());
    }

    [Test, Order(5)]
    public void AddPublisher_ReturnsBadRequest()
    {
      var newPublishervM = new PublisherVM
      {
        Name = "1New Publisher"
      };

      IActionResult actionResult = _publishersController.AddPublisher(newPublishervM);

      Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test, Order(6)]
    public void DeletePublisherById_ReturnsOk()
    {
      IActionResult actionResult = _publishersController.DeletePublisherById(1);

      Assert.That(actionResult, Is.TypeOf<OkResult>());
    }

    [Test, Order(7)]
    public void DeletePublisherById_ReturnsBadRequest()
    {
      IActionResult actionResult = _publishersController.DeletePublisherById(1);

      Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
    }
  }
}
