using System;
using System.Linq;
using Effortless.Net.Encryption;

namespace Common.Utilities
{
	class EncryptionService : IEncryptionService
	{
		private readonly IConfiguration _configuration;

		protected byte [] Key => Convert.FromBase64String(_configuration.Key);
		protected byte [] Iv => Convert.FromBase64String(_configuration.Iv);

		public EncryptionService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public bool TryEncrypt(string plain, out string encrypted)
		{
			var iv = Bytes.GenerateIV();
			var key = Bytes.GenerateKey();
			return TryEncryptInternal(plain, key, iv, out encrypted);
		}

		public bool TryEncryptSystem(string plain, out string encrypted)
		{
			return TryEncryptInternal(plain, Key, Iv, out encrypted);
		}

		public bool TryDecryptSystem(string encrypted, out string plain)
		{
			return TryDecryptInternal(encrypted, Key, Iv, out plain);
		}

		public bool TryDecrypt(string encrypted, out string plain)
		{
			plain = string.Empty;

			if (string.IsNullOrWhiteSpace(encrypted))
			{
				return true;
			}
			try
			{
				var bytes = Convert.FromBase64String(encrypted);
				var iv = bytes.Take(32).ToArray();
				var keyStart = bytes.Length - 32;

				var encryptedBytes = bytes.Skip(32).Take(bytes.Length - 64).ToArray();
				var key = bytes.Skip(keyStart).Take(32).ToArray();

				var encryptedString = Convert.ToBase64String(encryptedBytes);

				return TryDecryptInternal(encryptedString, key, iv, out plain);
			}
			catch
			{
				return false;
			}
		}

		private bool TryEncryptInternal(string plain, byte [] key, byte [] iv, out string encrypted)
		{
			encrypted = string.Empty;
			if (string.IsNullOrWhiteSpace(plain))
			{
				return true;
			}

			try
			{

				var encryptedString = Strings.Encrypt(plain, key, iv);

				var bytes = Convert.FromBase64String(encryptedString);

				var all = iv.Concat(bytes).Concat(key).ToArray();

				var fullEncryptedWithKeys = Convert.ToBase64String(all.ToArray());

				encrypted = fullEncryptedWithKeys;

				return true;
			}
			catch
			{
				return false;
			}
		}

		private bool TryDecryptInternal(string encryptedString, byte [] key, byte [] iv, out string plain)
		{
			plain = string.Empty;

			if (string.IsNullOrWhiteSpace(encryptedString))
			{
				return true;
			}
			try
			{
				var decrypted = Strings.Decrypt(encryptedString, key, iv);

				plain = decrypted;
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}