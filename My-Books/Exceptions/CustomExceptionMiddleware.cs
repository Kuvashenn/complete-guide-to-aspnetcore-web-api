﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using My_Books.Data.ViewModels;

namespace My_Books.Exceptions
{
  public class CustomExceptionMiddleware
  {
    private readonly RequestDelegate _next;

    public CustomExceptionMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
      httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      httpContext.Response.ContentType = "application/json";

      var response = new ErrorVM
      {
        StatusCode = httpContext.Response.StatusCode,
        Message = "Internal Server Error from the custom middleware",
        Path = "path goes here"
      };
      return httpContext.Response.WriteAsync(response.ToString());
    }
  }
}