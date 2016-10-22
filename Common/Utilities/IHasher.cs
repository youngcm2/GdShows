using Common.Models;

namespace Common.Utilities
{
	public interface IHasher
	{
		HashResult Hash(string value);
		bool Verify(string hash, string value, string salt);
	}
}