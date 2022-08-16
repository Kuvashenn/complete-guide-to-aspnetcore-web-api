using System.Collections.Generic;

namespace My_Books.Data.Models
{
  public class Publisher
  {
    public int Id { get; set; }
    public string Name { get; set; }
    
    //Navigation properties
    public List<Book> Books { get; set; }
  }
}
