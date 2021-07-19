using Entities;
using Entities.Models;
using FileUploadApi.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    try
                    {
                        var context = serviceProvider.GetRequiredService<RepositoryContext>();
                        context.Database.Migrate(); // migrate db

                        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                        // seed data
                        DataSeeder.SeedData(context, userManager, roleManager).Wait();

                    }
                    catch (Exception ex)
                    {
                        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    }
                }
                host.Run();

            }
            catch (Exception e)
            {
                throw;
            }

        }
        /*public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.Seq("")
                .WriteTo.File("log.txt")
                .WriteTo.MSSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=FileUpload;Integrated Security=SSPI",
                         new MSSqlServerSinkOptions
                         {
                             TableName = "Logs",
                             SchemaName = "dbo",
                             AutoCreateSqlTable = true
                         })
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }*/

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
