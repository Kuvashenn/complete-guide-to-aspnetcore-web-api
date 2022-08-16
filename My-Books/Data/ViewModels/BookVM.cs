using System;
using System.Collections.Generic;

namespace My_Books.Data.ViewModels
{
  public class BookVM
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRead { get; set; }
    public DateTime? DateRead { get; set; }
    public int? Rate { get; set; }
    public string Genre { get; set; }
    public string CoverUrl { get; set; }
    public int PublisherId { get; set; }
    public List<int> AuthorIds { get; set; }
  }

  public class BookWithAuthorsVM
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRead { get; set; }
    public DateTime? DateRead { get; set; }
    public int? Rate { get; set; }
    public string Genre { get; set; }
    public string CoverUrl { get; set; }
    public string PublisherName { get; set; }
    public List<string> AuthorsNames { get; set; }
  }
}