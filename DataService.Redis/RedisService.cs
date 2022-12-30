using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Redis
{
    public class RedisService : RedisContext, IRedisService
    {
        public ConnectionMultiplexer _conn;
        private readonly RedisOption _options;

        #region 构造函数
        public RedisService(IOptions<RedisOption> options) : base(options)
        {
            _conn = Instance;
            _options = options.Value;
        }
        #endregion 构造函数

        #region String
        #region 同步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.StringSet(key, value, expiry), dbNum);
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues, int dbNum = 0)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(p.Key), p.Value)).ToList();
            return Do(db => db.StringSet(newkeyValues.ToArray()), dbNum);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, expiry), dbNum);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T obj, DateTime expiry, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, expiry - DateTime.Now), dbNum);
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public string StringGet(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.StringGet(key), dbNum);
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public RedisValue[] StringGet(List<string> listKey, int dbNum = 0)
        {
            List<string> newKeys = listKey.Select(AddSysCustomKey).ToList();
            return Do(db => db.StringGet(ConvertRedisKeys(newKeys)), dbNum);
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T StringGet<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => ConvertObj<T>(db.StringGet(key)), dbNum);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double StringIncrement(string key, double val = 1, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.StringIncrement(key, val), dbNum);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double StringDecrement(string key, double val = 1, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.StringDecrement(key, val), dbNum);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.StringSetAsync(key, value, expiry), dbNum);
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues, int dbNum = 0)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(p.Key), p.Value)).ToList();
            return await Do(db => db.StringSetAsync(newkeyValues.ToArray()), dbNum);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return await Do(db => db.StringSetAsync(key, json, expiry), dbNum);
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.StringGetAsync(key), dbNum);
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public async Task<RedisValue[]> StringGetAsync(List<string> listKey, int dbNum = 0)
        {
            List<string> newKeys = listKey.Select(AddSysCustomKey).ToList();
            return await Do(db => db.StringGetAsync(ConvertRedisKeys(newKeys)), dbNum);
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            string result = await Do(db => db.StringGetAsync(key), dbNum);
            return ConvertObj<T>(result);

        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> StringIncrementAsync(string key, double val = 1, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.StringIncrementAsync(key, val), dbNum);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> StringDecrementAsync(string key, double val = 1, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.StringDecrementAsync(key, val), dbNum);
        }

        #endregion 异步方法

        #endregion String

        #region Hash

        #region 同步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashExists(string key, string dataKey, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashExists(key, dataKey), dbNum);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool HashSet<T>(string key, string dataKey, T t, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                string json = ConvertJson(t);
                return db.HashSet(key, dataKey, json);
            }, dbNum);
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashDelete(string key, string dataKey, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashDelete(key, dataKey), dbNum);
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public long HashDelete(string key, List<RedisValue> dataKeys, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return Do(db => db.HashDelete(key, dataKeys.ToArray()), dbNum);
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, string dataKey, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                string value = db.HashGet(key, dataKey);
                return ConvertObj<T>(value);
            }, dbNum);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double HashIncrement(string key, string dataKey, double val = 1, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashIncrement(key, dataKey, val), dbNum);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double HashDecrement(string key, string dataKey, double val = 1, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashDecrement(key, dataKey, val), dbNum);
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> HashKeys<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                RedisValue[] values = db.HashKeys(key);
                return ConvetList<T>(values);
            }, dbNum);
        }

        /// <summary>
        /// 获取hashkey所有Redis Values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> HashValues<T>(string key, int dbNum = 0)
        {
            var result = new List<T>();
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                HashEntry[] arr = db.HashGetAll(key);
                foreach (var item in arr)
                {
                    if (!item.Value.IsNullOrEmpty)
                    {
                        result.Add(JsonConvert.DeserializeObject<T>(item.Value));
                    }
                }
                return result;
            }, dbNum);
        }

        public HashEntry[] HashValueAll(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                HashEntry[] arr = db.HashGetAll(key);
                return arr;
            }, dbNum);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashExistsAsync(string key, string dataKey, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashExistsAsync(key, dataKey), dbNum);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string key, string dataKey, T t, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db =>
            {
                string json = ConvertJson(t);
                return db.HashSetAsync(key, dataKey, json);
            }, dbNum);
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string dataKey, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashDeleteAsync(key, dataKey), dbNum);
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return await Do(db => db.HashDeleteAsync(key, dataKeys.ToArray()), dbNum);
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<T> HashGeAsync<T>(string key, string dataKey, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            string value = await Do(db => db.HashGetAsync(key, dataKey), dbNum);
            return ConvertObj<T>(value);

        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashIncrementAsync(key, dataKey, val), dbNum);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashDecrementAsync(key, dataKey, val), dbNum);
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> HashKeysAsync<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            RedisValue[] values = await Do(db => db.HashKeysAsync(key), dbNum);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> HashValuesAsync<T>(string key, int dbNum = 0)
        {
            var result = new List<T>();
            key = AddSysCustomKey(key);
            HashEntry[] arr = await Do(db => db.HashGetAllAsync(key), dbNum);
            foreach (var item in arr)
            {
                string values = item.Name;
                if (!item.Value.IsNullOrEmpty)
                {
                    result.Add(JsonConvert.DeserializeObject<T>(item.Value));
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<HashEntry[]> HashValueAllAsync(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            HashEntry[] arr = await Do(db => db.HashGetAllAsync(key), dbNum);
            return arr;
        }
        #endregion 异步方法

        #endregion Hash

        #region List

        #region 同步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListRemove<T>(string key, T value, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            Do(db => db.ListRemove(key, ConvertJson(value)), dbNum);
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(redis =>
            {
                var values = redis.ListRange(key);
                return ConvetList<T>(values);
            }, dbNum);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListRightPush<T>(string key, T value, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            Do(db => db.ListRightPush(key, ConvertJson(value)), dbNum);
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListRightPop<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                var value = db.ListRightPop(key);
                return ConvertObj<T>(value);
            }, dbNum);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListLeftPush<T>(string key, T value, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            Do(db => db.ListLeftPush(key, ConvertJson(value)), dbNum);
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                var value = db.ListLeftPop(key);
                return ConvertObj<T>(value);
            }, dbNum);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(redis => redis.ListLength(key), dbNum);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRemoveAsync<T>(string key, T value, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.ListRemoveAsync(key, ConvertJson(value)), dbNum);
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> ListRangeAsync<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            var values = await Do(redis => redis.ListRangeAsync(key), dbNum);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRightPushAsync<T>(string key, T value, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.ListRightPushAsync(key, ConvertJson(value)), dbNum);
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            var value = await Do(db => db.ListRightPopAsync(key), dbNum);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListLeftPushAsync<T>(string key, T value, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.ListLeftPushAsync(key, ConvertJson(value)), dbNum);
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            var value = await Do(db => db.ListLeftPopAsync(key), dbNum);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(redis => redis.ListLengthAsync(key), dbNum);
        }

        #endregion 异步方法

        #endregion List

        #region SortedSet 有序集合

        #region 同步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public bool SortedSetAdd<T>(string key, T value, double score, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetAdd(key, ConvertJson<T>(value), score), dbNum);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool SortedSetRemove<T>(string key, T value, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetRemove(key, ConvertJson(value)), dbNum);
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> SortedSetRangeByRank<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(redis =>
            {
                var values = redis.SortedSetRangeByRank(key);
                return ConvetList<T>(values);
            }, dbNum);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSetLength(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetLength(key), dbNum);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(redis => redis.SortedSetAddAsync(key, ConvertJson<T>(value), score), dbNum);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(redis => redis.SortedSetRemoveAsync(key, ConvertJson(value)), dbNum);
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            var values = await Do(redis => redis.SortedSetRangeByRankAsync(key), dbNum);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return await Do(redis => redis.SortedSetLengthAsync(key), dbNum);
        }

        #endregion 异步方法

        #endregion SortedSet 有序集合

        #region key

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyDelete(key), dbNum);
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(List<string> keys, int dbNum = 0)
        {
            List<string> newKeys = keys.Select(AddSysCustomKey).ToList();
            return Do(db => db.KeyDelete(ConvertRedisKeys(newKeys)), dbNum);
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public bool KeyExists(string key, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExists(key), dbNum);
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey, int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyRename(key, newKey), dbNum);
        }

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExpire(key, expiry), dbNum);
        }

        /// <summary>
        /// 获取所有KEY
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCacheKeys(int dbNum = 0)
        {
            try
            {
                var result = new List<string>();
                var endpoints = _conn.GetEndPoints(true);
                foreach (var endpoint in endpoints)
                {
                    var server = _conn.GetServer(endpoint);
                    result.AddRange(server.Keys(dbNum).Select(x => x.ToString()));
                }
                return result.Distinct().ToList();
            }
            catch
            {
                return null;
            }
        }

        #endregion key

        #region 发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            ISubscriber sub = _conn.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
            {
                if (handler == null)
                {
                    Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                }
                else
                {
                    handler(channel, message);
                }
            });
        }

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long Publish<T>(string channel, T msg)
        {
            ISubscriber sub = _conn.GetSubscriber();
            return sub.Publish(channel, ConvertJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            ISubscriber sub = _conn.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            ISubscriber sub = _conn.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion 发布订阅

        #region 其他

        public ITransaction CreateTransaction()
        {
            return GetDatabase().CreateTransaction();
        }

        public IDatabase GetDatabase(int dbNum = 0)
        {
            return _conn.GetDatabase(dbNum);
        }

        public IServer GetServer(string hostAndPort)
        {
            return _conn.GetServer(hostAndPort);
        }
        #endregion 其他

        #region 辅助方法

        private string AddSysCustomKey(string oldKey)
        {
            var prefixKey = _options.RedisPrefixKey;
            return prefixKey + oldKey;
        }

        private T Do<T>(Func<IDatabase, T> func, int dbNum)
        {
            try
            {
                var database = _conn.GetDatabase(dbNum);
                return func(database);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"reids.do:{ex.Message}");
                return default(T);
            }
        }

        private string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        private T ConvertObj<T>(RedisValue value)
        {
            if (value.HasValue)
            {
                if (typeof(T).Name.Equals(typeof(string).Name))
                {
                    return JsonConvert.DeserializeObject<T>($"'{value}'");
                }
                else
                {
                    T t = JsonConvert.DeserializeObject<T>(value);
                    if (t == null)
                    {
                        Console.WriteLine($"reids.ConvertObj.t:{null}");
                    }
                    return t;
                }
            }
            return default(T);
        }


        private List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }

        #endregion 辅助方法
    }
}
