using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EveryoneToTheHackathon.Application;

class Program
{
    private static void Main(string[] args)
    {
       Host
           .CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(builder =>
           {    
               builder.UseStartup<Startup>();
           })
           .Build()
           .Run();
    }
}