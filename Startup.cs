using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Cosmos;
using Azure.Storage.Blobs;
using BackEnd.Entities;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // CORS policy for development, modify as needed for production
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        // Swagger configuration
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "YourApiName", Version = "v1" });
        });

        // Key Vault settings
        var keyVaultUri = Configuration["KeyVault:VaultUri"];

        // Create a SecretClient
        var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

        // Retrieve the secrets
        var cosmosConnectionString = client.GetSecret("CosmosConnection").Value.Value;
        var blobConnectionString = client.GetSecret("YourBlobConnectionString").Value.Value;

        // Cosmos DB configuration
        services.AddSingleton<CosmosClient>(x =>
        {
            return new CosmosClient(cosmosConnectionString);
        });

        // Register BlobServiceClient
        services.AddSingleton<BlobServiceClient>(x =>
        {
            return new BlobServiceClient(blobConnectionString);
        });

        // Register CosmosDbContext
        services.AddSingleton<CosmosDbContext>();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackEnd Api's");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        // CORS policy for development, modify as needed for production
        app.UseCors("AllowAll");

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
