﻿using EVF.Helper.Models;
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
        public static ResultViewModel InitialResultError(string message, int statusCode = 500, object modelState = null)
        {
            return new ResultViewModel
            {
                IsError = true,
                StatusCode = statusCode,
                Message = message,
                ModelError = modelState
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


        /// <summary>
        /// Calculate average score.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <param name="userTotal">The average.</param>
        /// <returns></returns>
        public static double AverageScore(double score, int userTotal)
        {
            return score / userTotal;
        }

        /// <summary>
        /// Calculate kpi group score.
        /// </summary>
        /// <param name="purScore">The purchasing score.</param>
        /// <param name="userScore">The average users score.</param>
        /// <param name="userPercentage">The user score percentage.</param>
        /// <param name="purchasePercentange">The purchase score percentage.</param>
        /// <returns></returns>
        public static double CalculateScore(double purScore, double userScore, int userPercentage, int purchasePercentange)
        {
            purScore = (purScore * purchasePercentange) / 100;
            userScore = (userScore * userPercentage) / 100;
            return Math.Round(purScore + userScore);
        }

        /// <summary>
        /// Get value object from dictionary to string.
        /// </summary>
        /// <param name="dic">The dictionary value.</param>
        /// <param name="key">The key target.</param>
        /// <returns></returns>
        public static string GetValueDictionaryToString(Dictionary<string,object> dic, string key)
        {
            return dic.GetValueOrDefault(key).ToString();
        }

        /// <summary>
        /// Get value object from dictionary to int.
        /// </summary>
        /// <param name="dic">The dictionary value.</param>
        /// <param name="key">The key target.</param>
        /// <returns></returns>
        public static int GetValueDictionaryToInt(Dictionary<string, object> dic, string key)
        {
            return Convert.ToInt32(dic.GetValueOrDefault(key).ToString());
        }

        /// <summary>
        /// Get value object from dictionary to datetime.
        /// </summary>
        /// <param name="dic">The dictionary value.</param>
        /// <param name="key">The key target.</param>
        /// <returns></returns>
        public static DateTime GetValueDictionaryToDateTime(Dictionary<string, object> dic, string key)
        {
            return DateTime.Parse(dic.GetValueOrDefault(key).ToString());
        }

        /// <summary>
        /// Write images file to server directory.
        /// </summary>
        /// <param name="imageList">The images model collection.</param>
        /// <param name="dataId">The data identity.</param>
        /// <param name="processCode">The process code folder.</param>
        /// <returns></returns>
        public static ResultViewModel SaveImages(IEnumerable<ImageViewModel> imageList, int dataId, string processCode)
        {
            var result = new ResultViewModel();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", processCode, dataId.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var item in imageList)
            {
                var file = Convert.FromBase64String(item.FileContent);
                string savePath = Path.Combine(path, item.FileName);
                File.WriteAllBytes(savePath, file);
            }
            return result;
        }

        /// <summary>
        /// Get images from id.
        /// </summary>
        /// <param name="dataId">The data identity.</param>
        /// <param name="processCode">The process code folder.</param>
        /// <returns></returns>
        public static IEnumerable<ImageViewModel> GetImages(int dataId, string processCode)
        {
            var result = new List<ImageViewModel>();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", processCode, dataId.ToString());
            if (Directory.Exists(path))
            {
                string[] allfiles = Directory.GetFiles(path);
                foreach (var item in allfiles)
                {
                    var fileByte = File.ReadAllBytes(item);
                    result.Add(new ImageViewModel
                    {
                        FileName = Path.GetFileName(item),
                        FileContent = Convert.ToBase64String(fileByte)
                    });
                }
            }
            return result;
        }

    }

}
