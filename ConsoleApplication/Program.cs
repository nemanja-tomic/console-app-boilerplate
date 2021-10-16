using System;
using System.IO;
using ConsoleApplication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ConsoleApplication
{
	public class Program
	{
		private static IConfigurationRoot _configuration;

		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(LogEventLevel.Debug, theme: AnsiConsoleTheme.Code)
				.MinimumLevel.Debug()
				.Enrich.FromLogContext()
				.CreateLogger();
			
			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);

			var serviceProvider = serviceCollection.BuildServiceProvider();
			var application = serviceProvider.GetService<Application>();
			
			try
			{
				Log.Information("Starting application");
				application?.Run();
				Log.Information("Application started");
			}
			catch (Exception e)
			{
				Log.Fatal(e, "Error running service");
				throw;
			}
			finally
			{
				Log.Information("Terminating application");
				application?.Dispose();
			}
		}

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			_configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
				.AddJsonFile("appsettings.json")
				.Build();

			serviceCollection.AddSingleton(_configuration);
			serviceCollection.Configure<ApplicationOptions>(_configuration.GetSection("ApplicationOptions"));

			serviceCollection.AddTransient<Application>();
		} 
	}
}