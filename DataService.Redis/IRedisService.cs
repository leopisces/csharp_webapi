using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Redis
{
    public interface IRedisService
    {
        #region String

        #region 同步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0);

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues, int dbNum = 0);

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool StringSet<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0);

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool StringSet<T>(string key, T obj, DateTime expiry, int dbNum = 0);

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        string StringGet(string key, int dbNum = 0);

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        RedisValue[] StringGet(List<string> listKey, int dbNum = 0);

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T StringGet<T>(string key, int dbNum = 0);

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        double StringIncrement(string key, double val = 1, int dbNum = 0);

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        double StringDecrement(string key, double val = 1, int dbNum = 0);


        #endregion

        #region 异步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0);

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0);

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        Task<string> StringGetAsync(string key, int dbNum = 0);

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        Task<RedisValue[]> StringGetAsync(List<string> listKey, int dbNum = 0);

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> StringGetAsync<T>(string key, int dbNum = 0);

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        Task<double> StringIncrementAsync(string key, double val = 1, int dbNum = 0);

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        Task<double> StringDecrementAsync(string key, double val = 1, int dbNum = 0);

        #endregion

        #endregion

        #region Hash

        #region 同步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        bool HashExists(string key, string dataKey, int dbNum = 0);

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        bool HashSet<T>(string key, string dataKey, T t, int dbNum = 0);

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        bool HashDelete(string key, string dataKey, int dbNum = 0);

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        long HashDelete(string key, List<RedisValue> dataKeys, int dbNum = 0);

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        T HashGet<T>(string key, string dataKey, int dbNum = 0);

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        double HashIncrement(string key, string dataKey, double val = 1, int dbNum = 0);

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        double HashDecrement(string key, string dataKey, double val = 1, int dbNum = 0);

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> HashKeys<T>(string key, int dbNum = 0);

        /// <summary>
        /// 获取hashkey所有对象Redis Values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> HashValues<T>(string key, int dbNum = 0);

        /// <summary>
        ///  获取hashkey所有Redis Values
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        HashEntry[] HashValueAll(string key, int dbNum = 0);

        #endregion

        #region 异步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        Task<bool> HashExistsAsync(string key, string dataKey, int dbNum = 0);

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> HashSetAsync<T>(string key, string dataKey, T t, int dbNum = 0);

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        Task<bool> HashDeleteAsync(string key, string dataKey, int dbNum = 0);

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys, int dbNum = 0);

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        Task<T> HashGeAsync<T>(string key, string dataKey, int dbNum = 0);

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        Task<double> HashIncrementAsync(string key, string dataKey, double val = 1, int dbNum = 0);

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        Task<double> HashDecrementAsync(string key, string dataKey, double val = 1, int dbNum = 0);

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<T>> HashKeysAsync<T>(string key, int dbNum = 0);

        /// <summary>
        /// 获取hashkey所有对象Redis Values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<T>> HashValuesAsync<T>(string key, int dbNum = 0);

        /// <summary>
        /// 获取hashkey所有Redis Values
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<HashEntry[]> HashValueAllAsync(string key, int dbNum = 0);

        #endregion

        #endregion

        #region List

        #region 同步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ListRemove<T>(string key, T value, int dbNum = 0);

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> ListRange<T>(string key, int dbNum = 0);

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ListRightPush<T>(string key, T value, int dbNum = 0);

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T ListRightPop<T>(string key, int dbNum = 0);

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ListLeftPush<T>(string key, T value, int dbNum = 0);

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T ListLeftPop<T>(string key, int dbNum = 0);

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListLength(string key, int dbNum = 0);

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<long> ListRemoveAsync<T>(string key, T value, int dbNum = 0);

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<T>> ListRangeAsync<T>(string key, int dbNum = 0);

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<long> ListRightPushAsync<T>(string key, T value, int dbNum = 0);

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> ListRightPopAsync<T>(string key, int dbNum = 0);

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
       Task<long> ListLeftPushAsync<T>(string key, T value, int dbNum = 0);

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> ListLeftPopAsync<T>(string key, int dbNum = 0);

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> ListLengthAsync(string key, int dbNum = 0);

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
        bool SortedSetAdd<T>(string key, T value, double score, int dbNum = 0);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SortedSetRemove<T>(string key, T value, int dbNum = 0);

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> SortedSetRangeByRank<T>(string key, int dbNum = 0);

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long SortedSetLength(string key, int dbNum = 0);

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        Task<bool> SortedSetAddAsync<T>(string key, T value, double score, int dbNum = 0);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SortedSetRemoveAsync<T>(string key, T value, int dbNum = 0);

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<T>> SortedSetRangeByRankAsync<T>(string key, int dbNum = 0);

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> SortedSetLengthAsync(string key, int dbNum = 0);

        #endregion 异步方法

        #endregion SortedSet 有序集合

        #region key

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key, int dbNum = 0);

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        long KeyDelete(List<string> keys, int dbNum = 0);

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public bool KeyExists(string key, int dbNum = 0);

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        bool KeyRename(string key, string newKey, int dbNum = 0);

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?), int dbNum = 0);

        /// <summary>
        /// 获取所有KEY
        /// </summary>
        /// <returns></returns>
        List<string> GetAllCacheKeys(int dbNum=0);

        #endregion key

        #region 发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null);

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        long Publish<T>(string channel, T msg);

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        void Unsubscribe(string channel);

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        void UnsubscribeAll();

        #endregion 发布订阅
    }
}
