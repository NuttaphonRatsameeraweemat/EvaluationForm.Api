using EVF.Helper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace EVF.Helper
{

    /// <summary>
    /// The Utility Service Class.
    /// </summary>
    public static class UtilityService
    {

        /// <summary>
        /// Initial Error Result and Message to return.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ResultViewModel InitialResultError(string message, int statusCode = 500)
        {
            return new ResultViewModel
            {
                IsError = true,
                StatusCode = statusCode,
                Message = message
            };
        }

        /// <summary>
        /// Encrption String with private key.
        /// </summary>
        /// <param name="text">The string text to encry.</param>
        /// <param name="keyString">The key encryption.</param>
        /// <returns></returns>
        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        /// <summary>
        /// Decryption string with private key.
        /// </summary>
        /// <param name="cipherText">The string text to decrypt.</param>
        /// <param name="keyString">The private key.</param>
        /// <returns></returns>
        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Serialize object class to string content.
        /// </summary>
        /// <typeparam name="T">The object class paramter.</typeparam>
        /// <param name="model">The object.</param>
        /// <returns></returns>
        public static StringContent SerializeContent<T>(T model)
        {
            var jsonString = JsonConvert.SerializeObject(model);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Deserialize string to object class.
        /// </summary>
        /// <typeparam name="T">The object class type.</typeparam>
        /// <param name="httpContent">The httpcontext.</param>
        /// <returns></returns>
        public static T DeserializeContent<T>(HttpContent httpContent)
        {
            var jsonString = httpContent.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// Convert string to date time using parameter format.
        /// </summary>
        /// <param name="value">The string datetime.</param>
        /// <param name="format">The datetime format.</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string value, string format)
        {
            return DateTime.TryParseExact(value, format,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None, out DateTime temp) ? temp : throw new ArgumentException($"DateTime incorrect format : {value}");
        }

    }

}
