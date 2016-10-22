namespace Common
{
	public interface IConfiguration
	{
		bool CheckForDatabase { get; }
		string ContextConnectionString { get; }
		string LogLevel { get; }
		string EnvName { get; }
		string SiteRoot { get; }
		string Key { get; }
		string Iv { get; }
		bool DebugMode { get; }
		bool MaintenanceMode { get; set; }
		void Reload();
	}
}