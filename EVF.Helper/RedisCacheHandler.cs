using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper
{
    public class RedisCacheHandler
    {
        #region [Fields]

        /// <summary>
        /// Redis cache ConnectionMultiplexer.
        /// </summary>
        private static ConnectionMultiplexer _connection;

        #endregion

        #region [Properties]
        /// <summary>
        /// Gets or sets the ConnectionString value.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Gets the Redis cache connection.
        /// </summary>
        private static ConnectionMultiplexer Connection
        {
            get
            {
                if (String.IsNullOrEmpty(ConnectionString))
                {
                    throw new InvalidOperationException("Missing ConnectionString property value.");
                }

                if (_connection == null)
                {
                    _connection = ConnectionMultiplexer.Connect(ConnectionString);
                }

                return _connection;
            }
        }
        #endregion

        #region [Methods]

        /// <summary>
        /// Get value from redis cache follow the input key.
        /// Incase the input key does not exists in the redis cache, can set the delegate action for set the value to redis cache.
        /// </summary>
        /// <typeparam name="TOutput">Class of output</typeparam>
        /// <param name="key">Key that need to get value from redis cache</param>
        /// <param name="setToCacheIfNotFound">Delegate function</param>
        /// <param name="expiryMinutes">Expiry time (Minutes) of key in Redis cache</param>
        /// <returns>Object with type as TOutput class</returns>
        public static TOutput GetValue<TOutput>(string key, Func<TOutput> setToCacheIfNotFound = null, int? expiryMinutes = null)
            where TOutput : class
        {
            //Initial return result
            TOutput result = null;
            //Get redis cache database
            IDatabase redisDatabase = Connection.GetDatabase();

            //Check input key exists in Redis cache or not.
            //If key does not exists, do the action follow setToCacheIfNotfound.
            //Else get string value from Redis cache and deserialize the string value to TOutput object.
            if (!redisDatabase.KeyExists(key))
            {
                //Check setToCacheIfNotFound input value.      
                //If the value is not null, run that function, and set the value into Redis cache.       
                if (setToCacheIfNotFound != null)
                {
                    //Run the input function.
                    result = setToCacheIfNotFound();

                    //Set processing result from setToCacheIfNotFound function into the key in Redis cache.
                    if (expiryMinutes.HasValue)
                    {
                        SetValue<TOutput>(key, result, expiryMinutes);
                    }
                    else
                    {
                        SetValue<TOutput>(key, result);
                    }
                }
            }
            else
            {
                //Deserialize string Redis cache value into TOutput object.
                result = JsonConvert.DeserializeObject<TOutput>(redisDatabase.StringGet(key));
            }

            return result;
        }

        /// <summary>
        /// Set value to redis cache.
        /// </summary>
        /// <typeparam name="T">Class of input value</typeparam>
        /// <param name="key">Key of data that need to set the value</param>
        /// <param name="value">Value that need to set to data in redis cache</param>
        /// <param name="expiryMinutes">Expiry time (Minutes) of key in Redis cache</param>
        public static void SetValue<T>(string key, T value, int? expiryMinutes = null) where T : class
        {
            //Get redis cache database
            IDatabase redisDatabase = Connection.GetDatabase();
            //Set value to Redis cache follow the key.
            redisDatabase.StringSet(key, JsonConvert.SerializeObject(value));

            if (expiryMinutes.HasValue)
            {
                //Set expiry time to the key.
                redisDatabase.KeyExpire(key, new TimeSpan(0, expiryMinutes.GetValueOrDefault(), 0));
            }
        }

        /// <summary>
        /// Delete cache data following the input key.
        /// </summary>
        /// <param name="key">Cache key that need to delete the data.</param>
        public static bool ClearValue(string key)
        {
            //Get redis cache database
            IDatabase redisDatabase = Connection.GetDatabase();
            return redisDatabase.KeyDelete(key);
        }

        #endregion
    }
}
