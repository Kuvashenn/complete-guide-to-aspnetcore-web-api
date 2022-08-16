using System;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using My_Books.ActionResults;
using My_Books.Data.Models;
using My_Books.Data.Services;
using My_Books.Data.ViewModels;
using My_Books.Exceptions;

namespace My_Books.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PublishersController : ControllerBase
  {
    private readonly PublishersService _publishersService;
    private readonly ILogger<PublishersController> _logger;

    public PublishersController(PublishersService publishersService, ILogger<PublishersController> logger)
    {
      _publishersService = publishersService;
      _logger = logger;
    }

    [HttpGet("get-all-publishers")]
    public IActionResult GetAllPublishers(string sortBy, string searchString, int pageNumber)
    {
     // throw new Exception("This is an exception thrown from get-all-publishers()");
      try
      {
        _logger.LogInformation("This is just a logger in get all publishers");
        var _result = _publishersService.GetAllPublishers(sortBy, searchString, pageNumber);
        return Ok(_result);
      }
      catch (Exception)
      {
        return BadRequest("Sorry we could load the publishers");
      }
    }

    [HttpPost("add-publisher")]
    public IActionResult AddPublisher([FromBody] PublisherVM publisher)
    {
      try
      {
        var newPublisher = _publishersService.AddPublisher(publisher);
        return Created(nameof(AddPublisher), newPublisher);
      }
      catch (PublisherNameException ex)
      {
        return BadRequest($"{ex.Message}, Publisher name: {ex.PublisherName}");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("get-publisher-books-with-authors/{id}")]
    public IActionResult GetPublisherData(int id)
    {
      var _response = _publishersService.GetPublisherData(id);
      return Ok(_response);
    }

    [HttpDelete("delete-publisher-by-id/{id}")]
    public IActionResult DeletePublisherById(int id)
    {
      try
      {
        _publishersService.DeletePublisherById(id);
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("get-publisher-by-id/{id}")]
    public IActionResult GetPublisherById(int id)
    {
      var response = _publishersService.GetPublisherById(id);

      if (response != null)
      {
        return Ok(response);
        //var _responseObj = new CustomActionResultVM()
        //{
        //  Publisher = response
        //};
        //return new CustomActionResult(_responseObj);
      }

      return NotFound();
    //  var _responseObj2 = new CustomActionResultVM()
    //  {
    //    Exception = new Exception("This is coming from Publishers Controller")
    //  };
    //  return new CustomActionResult(_responseObj2);
    }
  }
}
