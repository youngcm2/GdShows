using Microsoft.Extensions.Configuration;

namespace Common
{
	public class Configuration : IConfiguration
	{
		private readonly IConfigurationRoot _configurationRoot;

		public Configuration(IConfigurationRoot configurationRoot)
		{
			_configurationRoot = configurationRoot;
			SetValues();
		}

		public string SiteRoot { get; private set; }
		public bool CheckForDatabase { get; private set; }
		public string ContextConnectionString { get; private set; }
		public string LogLevel { get; private set; }
		public string EnvName { get; private set; }
		public string Key { get; private set; }
		public string Iv { get; private set; }
		public bool DebugMode { get; private set; }
		public bool MaintenanceMode { get; set; }

		public void Reload()
		{
			_configurationRoot.Reload();
			SetValues();
		}

		private void SetValues()
		{
			CheckForDatabase = GetBool("checkForDatabase");
			DebugMode = GetBool("DebugMode");
			MaintenanceMode = GetBool("MaintenanceMode");
			ContextConnectionString = Get("connectionStrings:context");
			LogLevel = Get("logging:level");
			EnvName = Get("envName");
			SiteRoot = Get("web:siteRoot");

			Key = Get("security:key");
			Iv = Get("security:iv");
		}

		public string Get(string name, string defaultValue = null)
		{
			var appSetting = _configurationRoot [name];
			return appSetting ?? defaultValue;
		}

		public bool GetBool(string name, bool defaultValue = false)
		{
			var value = Get(name);
			if (string.IsNullOrEmpty(value))
				return defaultValue;
			bool b;
			return bool.TryParse(value, out b) ? b : defaultValue;
		}
		public int? GetInt(string name, int? defaultValue = null)
		{
			var value = Get(name);
			if (string.IsNullOrEmpty(value))
				return defaultValue;
			int i;
			return int.TryParse(value, out i) ? i : defaultValue;
		}
	}
}