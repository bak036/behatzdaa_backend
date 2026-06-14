using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Nofshonit.Infrastructure.Utils
{
	public static class Cryptor
	{
		public static string EncryptionKey = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<string>("EncryptionKey");

		public static string MD5Encrypt(string password)
		{
			password = password.Trim();
			ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			byte[] data = encoding.GetBytes(password);
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(data);
			password = encoding.GetString(result);
			password = password.Replace("'", "''");
			return password;
		}


		public static string ConvertStringToHex(String input, System.Text.Encoding encoding)
		{
			Byte[] stringBytes = encoding.GetBytes(input);
			StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
			foreach (byte b in stringBytes)
			{
				sbBytes.AppendFormat("{0:X2}", b);
			}
			return sbBytes.ToString();
		}

		public static string ConvertHexToString(String hexInput, System.Text.Encoding encoding)
		{
			int numberChars = hexInput.Length;
			byte[] bytes = new byte[numberChars / 2];
			for (int i = 0; i < numberChars; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
			}
			return encoding.GetString(bytes);
		}

		public static string Decrypt(string cipherText)
		{
			if (cipherText != null)
				cipherText = cipherText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}

		public static string Encrypt(string clearText)
		{

			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}
	}
}
