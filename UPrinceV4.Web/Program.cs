using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace UPrinceV4.Web;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    //    WebHost.CreateDefaultBuilder(args)
    //        .UseStartup<Startup>();
    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        // XmlDocument log4netConfig = new XmlDocument();
        // log4netConfig.Load(File.OpenRead("log4net.config"));
        // log4net.Config.XmlConfigurator.Configure(log4net.LogManager.GetRepository(Assembly.GetEntryAssembly()), log4netConfig["log4net"]);
        
        var logRepository = LogManager.GetRepository ( Assembly . GetEntryAssembly ());
        XmlConfigurator . Configure (logRepository, new FileInfo ("log4net.config"));

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var webHostBuilder = WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
            .UseStartup<Startup>();
        return webHostBuilder;
    }
}