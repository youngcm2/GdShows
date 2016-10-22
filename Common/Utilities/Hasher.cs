using Common.Models;
using Effortless.Net.Encryption;

namespace Common.Utilities
{
	class Hasher : IHasher
	{
		public HashResult Hash(string value)
		{
			var salt = Strings.CreateSalt(20);

			var valueAndSalt = $"{value}{salt}";

			var hash = Effortless.Net.Encryption.Hash.Create(HashType.SHA256, valueAndSalt, string.Empty, false);

			return new HashResult
			{
				Hash = hash,
				Salt = salt
			};
		}

		public bool Verify(string hash, string value, string salt)
		{
			var valueAndSalt = $"{value}{salt}";

			var verify = Effortless.Net.Encryption.Hash.Verify(HashType.SHA256, valueAndSalt, string.Empty, false, hash);

			return verify;
		}
	}
}