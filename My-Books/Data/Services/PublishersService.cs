using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using My_Books.Data.Models;
using My_Books.Data.Paging;
using My_Books.Data.ViewModels;
using My_Books.Exceptions;

namespace My_Books.Data.Services
{
  public class PublishersService
  {
    private readonly AppDbContext _context;

    public PublishersService(AppDbContext context)
    {
      _context = context;
    }

    public Publisher AddPublisher(PublisherVM publisher)
    {
      if (StringStartsWithNumber(publisher.Name))
        throw new PublisherNameException("Name starts with number", publisher.Name);
      var _publisher = new Publisher
      {
        Name = publisher.Name
      };
      _context.Publishers.Add(_publisher);
      _context.SaveChanges();
      return _publisher;
    }

    public Publisher GetPublisherById(int id)
    {
      return _context.Publishers.FirstOrDefault(n => n.Id == id);
    }

    public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
    {
      var _publisherData = _context.Publishers.Where(n => n.Id == publisherId)
        .Select(n => new PublisherWithBooksAndAuthorsVM()
        {
          Name = n.Name,
          BookAuthors = n.Books.Select(n => new BookAuthorVM()
          {
            BookName = n.Title,
            BookAuthors = n.Book_Authors.Select(n => n.Author.FullName).ToList()
          }).ToList()
        }).FirstOrDefault();

      return _publisherData;
    }

    public void DeletePublisherById(int id)
    {
      var publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);

      if(publisher == null) return;

      _context.Publishers.Remove(publisher);
      _context.SaveChanges();
    }

    private bool StringStartsWithNumber(string name)
    {
      return Regex.IsMatch(name, @"^\d");
    }

    public IList<Publisher> GetAllPublishers(string sortBy, string searchString, int? pageNumber)
    {
      var allPublishers = _context.Publishers.OrderBy(n => n.Name).ToList();
      if (!string.IsNullOrWhiteSpace(sortBy))
      {
        switch (sortBy)
        {
          case "name_desc":
            allPublishers = allPublishers.OrderByDescending(n => n.Name).ToList();
            break;
          default:
            break;
        }
      }

      if (!string.IsNullOrWhiteSpace(searchString))
      {
        allPublishers = allPublishers.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
      }

      //Paging
      int pageSize = 5;
      allPublishers = PaginatedList<Publisher>.Create(allPublishers.AsQueryable(), pageNumber ?? 1, pageSize);
      return allPublishers;
    }
  }
}