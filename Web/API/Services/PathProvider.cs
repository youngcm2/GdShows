using System.IO;
using Common;
using Common.Services;
using Microsoft.AspNetCore.Hosting;

namespace GdShows.API.Services
{
	class PathProvider : IPathProvider
	{
		private readonly IConfiguration _configuration;
		private readonly IHostingEnvironment _hostingEnvironment;

		public PathProvider(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
		{
			_configuration = configuration;
			_hostingEnvironment = hostingEnvironment;
		}

		public string BuildAbsoluteUrl(string path)
		{
			var baseUrl = _configuration.SiteRoot;

			if (string.IsNullOrWhiteSpace(path))
			{
				return baseUrl;
			}

			if (path.StartsWith("/"))
			{
				return $"{baseUrl}{path}";
			}

			return $"{baseUrl}/{path}";
		}

		public string MapPath(string relativePath)
		{
			relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar).Replace("~", string.Empty);
			return Path.Combine(_hostingEnvironment.ContentRootPath, relativePath);
		}

		public string GetBaseUrl()
		{
			return _configuration.SiteRoot;
		}
	}
}