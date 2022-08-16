using System.Collections.Generic;

namespace My_Books.Data.ViewModels
{
  public class AuthorVM
  {
    public string Fullname { get; set; }
  }

  public class AuthoWithBooksVM
  {
    public string FullName { get; set; }
    public List<string> BookTitles { get; set; }
  }
}