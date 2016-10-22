namespace GdShows.API.Services
{
	public interface ISecurityTokenService
	{
		string Encrypt(SecurityToken securityToken);
		SecurityToken Decrypt(string encrypted);
	}
}