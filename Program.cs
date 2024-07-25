using Microsoft.AspNetCore.Hosting;

namespace BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureAppConfiguration((hostingContext, config) =>
          {
              // Add additional configuration sources as needed
          })
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Startup>();
          })
          .ConfigureServices((hostContext, services) =>
          {
              services.AddCors(options =>
              {
                  options.AddPolicy("AllowAll",
                      builder => builder
                          .AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
              });

              //services.AddDbContext<SocialMediaDbContext>(options =>
              //    options.UseCosmos(
              //        hostContext.Configuration.GetConnectionString("CosmosDb"),
              //        databaseName: "alluredb"
              //    )
              //);
              //using (var scope = services.BuildServiceProvider().CreateScope())
              //{
              //    var context = scope.ServiceProvider.GetRequiredService<SocialMediaDbContext>();
              //    SeedData.Initialize(context);
              //}
              services.AddControllers();
              services.AddEndpointsApiExplorer();
              services.AddSwaggerGen();
          });
    }
}