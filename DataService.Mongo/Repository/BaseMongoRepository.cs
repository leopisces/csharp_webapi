using DataService.Mongo.IRepository;
using DataService.Mongo.Models;
using DataService.Mongo.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataService.Mongo.Repository
{
    /// <summary>
    /// Mongo数据库接口实现类
    /// </summary>
    public class BaseMongoRepository: MongoDbContext, IBaseMongoRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public BaseMongoRepository(IOptions<MongoConfig> options) : base(options)
        {
        }

        #region 私有方法
        /// <summary>
        /// 设置实体Id
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="document">The document.</param>
        private void FormatDocument<TDocument, TKey>(TDocument document) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            var defaultTKey = default(TKey);
            if (document.Id == null
                || defaultTKey != null
                    && defaultTKey.Equals(document.Id))
            {
                document.Id = IdGenerator.GetId<TKey>();
            }
        }

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        private IMongoCollection<TDocument> HandlePartitioned<TDocument, TKey>(TDocument document) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            if (document is IPartitionedDocument)
            {
                return GetCollection<TDocument, TKey>(((IPartitionedDocument)document).PartitionKey);
            }
            return GetCollection<TDocument, TKey>();
        }

        /// <summary>
        /// Gets a collections for the type TDocument with a partition key.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="partitionKey">The collection partition key.</param>
        /// <returns></returns>
        private IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            return Database.GetCollection<TDocument>(partitionKey);
        }

        #endregion

        #region 异步方法
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AddOneAsync<TDocument, TKey>(TDocument document, CancellationToken cancellationToken = default) 
            where TDocument : IDocument<TKey> 
            where TKey : IEquatable<TKey>
        {
            FormatDocument<TDocument, TKey>(document);
            await HandlePartitioned<TDocument, TKey>(document).InsertOneAsync(document, null, cancellationToken);
        }

        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="documents"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AddManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!documents.Any())
            {
                return;
            }
            foreach (var document in documents)
            {
                FormatDocument<TDocument, TKey>(document);
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    await HandlePartitioned<TDocument, TKey>(group.FirstOrDefault()).InsertManyAsync(group.ToList(), null, cancellationToken);
                }
            }
            else
            {
                await GetCollection<TDocument, TKey>().InsertManyAsync(documents.ToList(), null, cancellationToken);
            }
        }
        #endregion

        #region 同步方法
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="document"></param>
        public void AddOne<TDocument, TKey>(TDocument document)where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            FormatDocument<TDocument, TKey>(document);
            HandlePartitioned<TDocument, TKey>(document).InsertOne(document);
        }

        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="documents"></param>
        public void AddMany<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!documents.Any())
            {
                return;
            }
            foreach (var document in documents)
            {
                FormatDocument<TDocument, TKey>(document);
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    HandlePartitioned<TDocument, TKey>(group.FirstOrDefault()).InsertMany(group.ToList());
                }
            }
            else
            {
                GetCollection<TDocument, TKey>().InsertMany(documents.ToList());
            }
        }
        #endregion
    }
}
