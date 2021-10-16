using System;
using Microsoft.Extensions.Options;

namespace ConsoleApplication
{
	public class Application : IDisposable
	{
		private readonly ApplicationOptions _options;

		public Application(IOptions<ApplicationOptions> options)
		{
			_options = options.Value;
		}
		
		public void Run()
		{
		}

		public void Dispose()
		{
		}
	}
}