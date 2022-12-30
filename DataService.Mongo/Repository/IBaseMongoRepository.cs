using DataService.Mongo.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataService.Mongo.IRepository
{
    /// <summary>
    /// Mongo数据库基类接口
    /// </summary>
    /// <typeparam name="TKey">The type of the document Id.</typeparam>
    public interface IBaseMongoRepository
    {
        #region 异步方法
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="document">添加的记录</param>
        /// <param name="cancellationToken">CancellationToken</param>
        Task AddOneAsync<TDocument, TKey>(TDocument document, CancellationToken cancellationToken = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task AddManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents, CancellationToken cancellationToken = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;
        #endregion

        #region 同步方法
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="document"></param>
        void AddOne<TDocument,TKey>(TDocument document) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documents"></param>
        void AddMany<TDocument, TKey>(IEnumerable<TDocument> documents) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;
        #endregion
    }
}
