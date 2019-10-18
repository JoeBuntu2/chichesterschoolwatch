using DataImporter.ImportTasks;
using DataImporter.MySQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DataImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder() 
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            var config = builder.Build();


            var connectionString = config.GetConnectionString("readonly");
            var services = new ServiceCollection();
            services.AddDbContext<SchoolDbContext>(options => options.UseMySQL(connectionString));

            //setup our DI
            var serviceProvider = services
                .AddSingleton<SchoolDbContext>() 
                .BuildServiceProvider();

            var tasks = new TaskCatalogue().GetTasks();
            while (true)
            {
                Console.WriteLine("Tasks: ");
                for (var index = 0; index < tasks.Length; index++)
                {
                    Console.WriteLine($"   {index} {tasks[index].GetType().Name}");
                }
                Console.Write("Enter Task Number: ");
                var option = int.Parse(Console.ReadLine());
                tasks[option].Run(serviceProvider);
            }             
        }


    }
}
