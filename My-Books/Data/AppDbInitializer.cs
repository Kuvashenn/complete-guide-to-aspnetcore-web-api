using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using My_Books.Data.Models;

namespace My_Books.Data
{
  public class AppDbInitializer
  {
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
      using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
      var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

      if (context!.Books.Any()) return;
      context.Books.AddRange(new Book()
        {
          Title = "1st Book Title",
          Description = "First book description",
          IsRead = true,
          DateRead = DateTime.Now.AddDays(-10),
          Rate = 4,
          Genre = "Biography",
          CoverUrl = "https...",
          DateAdded = DateTime.Now

        },
        new Book()
        {
          Title = "2nd Book Title",
          Description = "Second book description",
          IsRead = false,
          Genre = "Biography",
          CoverUrl = "https...",
          DateAdded = DateTime.Now
        });

      context.SaveChanges();
    }
  }
}