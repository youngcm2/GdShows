namespace Common.Services
{
	public interface IPathProvider
	{
		string BuildAbsoluteUrl(string path);
		string MapPath(string relativePath);
		string GetBaseUrl();
	}
}
