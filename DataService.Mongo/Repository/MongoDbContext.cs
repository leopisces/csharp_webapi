using DataService.Mongo.IRepository;
using DataService.Mongo.Models;
using DataService.Mongo.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DataService.Mongo.Repository
{
    /// <summary>
    /// Mongo数据库上下文
    /// </summary>
    public class MongoDbContext
    {
        /// <summary>
        /// 配置
        /// </summary>
        private readonly MongoConfig _config;

        /// <summary>
        /// 返回一个Database
        /// </summary>
        public IMongoDatabase Database
        {
            get
            {
                return GetDataBase();
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="databaseName">数据库名称</param>
        public MongoDbContext(IOptions<MongoConfig> options)
        {
            //InitializeGuidRepresentation();
            _config = options.Value;
        }

        /// <summary>
        /// 返回一个实例
        /// </summary>
        /// <returns></returns>
        public IMongoDatabase GetDataBase()
        {
            var client = new MongoClient(_config.ConnectionString);
            return client.GetDatabase(_config.Database);
        }

        /// <summary>
        /// Initialize the Guid representation of the MongoDB Driver.
        /// Override this method to change the default GuidRepresentation.
        /// </summary>
        protected void InitializeGuidRepresentation()
        {
            // by default, avoid legacy UUID representation: use Binary 0x04 subtype.
            //MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }
    }
}
