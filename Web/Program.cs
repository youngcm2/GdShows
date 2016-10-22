using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace GdShows
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = new WebHostBuilder()
		        .UseKestrel()
		        .UseContentRoot(Directory.GetCurrentDirectory())
		        .UseIISIntegration()
		        .UseStartup<Startup>();

#if DEBUG
			builder.UseUrls("http://0.0.0.0:1854");
#endif

			var host = builder
                .Build();

            host.Run();
        }
    }
}
