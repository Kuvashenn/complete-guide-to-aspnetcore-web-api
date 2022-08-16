using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using My_Books.Data;
using My_Books.Data.Services;
using My_Books.Exceptions;

namespace My_Books
{
  public class Startup
  {
    public string ConnectionString { get; set; }
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddControllers();
      // Configure DB Context with SQL
      services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

      // Configure the Services
      services.AddTransient<BooksService>();
      services.AddTransient<AuthorsService>();
      services.AddTransient<PublishersService>();
      services.AddTransient<LogsService>();
      services.AddApiVersioning(config =>
      {
        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;

        //config.ApiVersionReader = new HeaderApiVersionReader("customer-version-header");
        //config.ApiVersionReader = new MediaTypeApiVersionReader();
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "My_Books_Updated_Title", Version = "v2" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "My_Books_ui_updated v2"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();
      
      //Exception Handling 
      app.ConfigureBuildInExceptionHandler(loggerFactory);
      //app.ConfigureCustomExceptionHandler();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      //AppDbInitializer.Seed(app);
    }
  }
}
