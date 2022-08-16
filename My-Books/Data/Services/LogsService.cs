using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using My_Books.Data.Models;

namespace My_Books.Data.Services
{
  public class LogsService
  {
    private readonly AppDbContext _context;

    public LogsService(AppDbContext context)
    {
      _context = context;
    }

    public List<Log> GetAllLogsFromDb() => _context.Logs.ToList();
  }
}
