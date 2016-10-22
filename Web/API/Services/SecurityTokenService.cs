using Common.Utilities;
using Newtonsoft.Json;

namespace GdShows.API.Services
{
	class SecurityTokenService : ISecurityTokenService
	{
	    private readonly IEncryptionService _encryptionService;

		public SecurityTokenService(IEncryptionService encryptionService)
		{
			_encryptionService = encryptionService;
		}
		
	    public string Encrypt(SecurityToken securityToken)
	    {
			var json = JsonConvert.SerializeObject(securityToken);
			string encryptedToken;
			_encryptionService.TryEncrypt(json, out encryptedToken);

		    return encryptedToken;
	    }

	    public SecurityToken Decrypt(string encrypted)
	    {
		    if (string.IsNullOrWhiteSpace(encrypted))
		    {
			    return null;
		    }

			string decryptedToken;
		    if (!_encryptionService.TryDecrypt(encrypted, out decryptedToken))
		    {
			    return null;
		    }

		    var token = JsonConvert.DeserializeObject<SecurityToken>(decryptedToken);
		    return token;
	    }
    }
}
