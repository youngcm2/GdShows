namespace Common.Utilities
{
	public interface IEncryptionService
	{
		bool TryEncrypt(string plain, out string encrypted);
		bool TryEncryptSystem(string plain, out string encrypted);
		bool TryDecryptSystem(string encrypted, out string plain);
		bool TryDecrypt(string encrypted, out string plain);
	}
}