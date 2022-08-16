using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Books.Data.Services;

namespace My_Books.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LogsController : ControllerBase
  {
    private readonly LogsService _logsService;

    public LogsController(LogsService logsService)
    {
      _logsService = logsService;
    }

    [HttpGet("get-all-logs-from-db")]
    public IActionResult GetAllLogsFromDb()
    {
      try
      {
        return Ok(_logsService.GetAllLogsFromDb());
      }
      catch (Exception e)
      {
        return BadRequest("Could not load logs from the database");
      }
    }
  }
}
