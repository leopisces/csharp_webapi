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

namespace DataService.Mongo.Temp
{
    /// <summary>
    /// The MongoDb context
    /// </summary>
    public class MongoDbContext1
    {
        /// <summary>
        /// 配置
        /// </summary>
        private readonly MongoConfig _config;
        /// <summary>
        /// The IMongoDatabase from the official MongoDB driver
        /// </summary>
        public IMongoDatabase Database
        {
            get
            {
                return GetDataBase();
            }
        }

        /// <summary>
        /// The constructor of the MongoDbContext, it needs a connection string and a database name. 
        /// </summary>
        /// <param name="connectionString">The connections string.</param>
        /// <param name="databaseName">The name of your database.</param>
        public MongoDbContext1(IOptions<MongoConfig> options)
        {
            //InitializeGuidRepresentation();
            _config = options.Value;
        }

        public IMongoDatabase GetDataBase()
        {
            var client = new MongoClient(_config.ConnectionString);
            return client.GetDatabase(_config.Database);
        }

        /// <summary>
        /// Returns a collection for a document type. Also handles document types with a partition key.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="partitionKey">The optional value of the partition key.</param>
        public IMongoCollection<TDocument> GetCollection<TDocument>(string partitionKey = null)
        {
            return Database.GetCollection<TDocument>(GetCollectionName<TDocument>(partitionKey));
        }

        /// <summary>
        /// Drops a collection, use very carefully.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        public void DropCollection<TDocument>(string partitionKey = null)
        {
            Database.DropCollection(GetCollectionName<TDocument>(partitionKey));
        }

        /// <summary>
        /// Sets the Guid representation of the MongoDB Driver.
        /// </summary>
        /// <param name="guidRepresentation">The new value of the GuidRepresentation</param>
        public void SetGuidRepresentation(GuidRepresentation guidRepresentation)
        {
            //MongoDefaults.GuidRepresentation = guidRepresentation;
            //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonSerializer.RegisterSerializer(new GuidSerializer(guidRepresentation));
        }

        /// <summary>
        /// Extracts the CollectionName attribute from the entity type, if any.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <returns>The name of the collection in which the TDocument is stored.</returns>
        protected string GetAttributeCollectionName<TDocument>()
        {
            return (typeof(TDocument).GetTypeInfo()
                                     .GetCustomAttributes(typeof(CollectionNameAttribute))
                                     .FirstOrDefault() as CollectionNameAttribute)?.Name;
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

        /// <summary>
        /// Given the document type and the partition key, returns the name of the collection it belongs to.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
	    /// <param name="partitionKey">The value of the partition key.</param>
        /// <returns>The name of the collection.</returns>
        protected string GetCollectionName<TDocument>(string partitionKey)
        {
            var collectionName = GetAttributeCollectionName<TDocument>() ?? Pluralize<TDocument>();
            if (string.IsNullOrEmpty(partitionKey))
            {
                return collectionName;
            }
            return $"{partitionKey}-{collectionName}";
        }

        /// <summary>
        /// Very naively pluralizes a TDocument type name.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <returns>The pluralized document name.</returns>
        protected string Pluralize<TDocument>()
        {
            return typeof(TDocument).Name.Pluralize().Camelize();
        }

        #region 方法

        #region Utility Methods

        /// <summary>
        /// Gets a IMongoQueryable for a potentially partitioned document type and a filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">The filter definition.</param>
        /// <param name="partitionKey">The collection partition key.</param>
        /// <returns></returns>
        public IMongoQueryable<TDocument> GetQuery<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).AsQueryable().Where(filter);
        }

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public IMongoCollection<TDocument> HandlePartitioned<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
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
        public IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return Database.GetCollection<TDocument>(partitionKey);
        }

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="partitionKey">The collection partition key.</param>
        /// <returns></returns>
        public IMongoCollection<TDocument> HandlePartitioned<TDocument, TKey>(string partitionKey)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!string.IsNullOrEmpty(partitionKey))
            {
                return GetCollection<TDocument, TKey>(partitionKey);
            }
            return GetCollection<TDocument, TKey>();
        }

        /// <summary>
        /// Converts a LINQ expression of TDocument, TValue to a LINQ expression of TDocument, object
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expression">The expression to convert</param>
        protected Expression<Func<TDocument, object>> ConvertExpression<TDocument, TValue>(Expression<Func<TDocument, TValue>> expression)
        {
            var param = expression.Parameters[0];
            Expression body = expression.Body;
            var convert = Expression.Convert(body, typeof(object));
            return Expression.Lambda<Func<TDocument, object>>(convert, param);
        }

        /// <summary>
        /// Maps a IndexCreationOptions object to a MongoDB.Driver.CreateIndexOptions object
        /// </summary>
        /// <param name="indexCreationOptions">The options for creating an index.</param>
        /// <returns></returns>
        protected CreateIndexOptions MapIndexOptions(IndexCreationOptions indexCreationOptions)
        {
            return new CreateIndexOptions
            {
                Unique = indexCreationOptions.Unique,
                TextIndexVersion = indexCreationOptions.TextIndexVersion,
                SphereIndexVersion = indexCreationOptions.SphereIndexVersion,
                Sparse = indexCreationOptions.Sparse,
                Name = indexCreationOptions.Name,
                Min = indexCreationOptions.Min,
                Max = indexCreationOptions.Max,
                LanguageOverride = indexCreationOptions.LanguageOverride,
                ExpireAfter = indexCreationOptions.ExpireAfter,
                DefaultLanguage = indexCreationOptions.DefaultLanguage,
                Bits = indexCreationOptions.Bits,
                Background = indexCreationOptions.Background,
                Version = indexCreationOptions.Version
            };
        }


        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        protected IFindFluent<TDocument, TDocument> GetMinMongoQuery<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
                    where TDocument : IDocument<TKey>
                    where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                               .SortBy(ConvertExpression(minValueSelector))
                                                               .Limit(1);
        }

        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by descending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        protected IFindFluent<TDocument, TDocument> GetMaxMongoQuery<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
                    where TDocument : IDocument<TKey>
                    where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                               .SortByDescending(ConvertExpression(maxValueSelector))
                                                               .Limit(1);
        }


        #endregion

        #region Create TKey

        /// <summary>
        /// Asynchronously adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task AddOneAsync<TDocument, TKey>(TDocument document, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            FormatDocument<TDocument, TKey>(document);
            await HandlePartitioned<TDocument, TKey>(document).InsertOneAsync(document, null, cancellationToken);
        }

        /// <summary>
        /// Adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        public void AddOne<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            FormatDocument<TDocument, TKey>(document);
            HandlePartitioned<TDocument, TKey>(document).InsertOne(document);
        }

        /// <summary>
        /// Asynchronously adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
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

        /// <summary>
        /// Adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
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

        #region Delete TKey

        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteOne<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", document.Id);
            return HandlePartitioned<TDocument, TKey>(document).DeleteOne(filter).DeletedCount;
        }

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteOneAsync<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", document.Id);
            return (await HandlePartitioned<TDocument, TKey>(document).DeleteOneAsync(filter)).DeletedCount;
        }

        /// <summary>
        /// Deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteOne<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).DeleteOne(filter).DeletedCount;
        }

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return (await HandlePartitioned<TDocument, TKey>(partitionKey).DeleteOneAsync(filter)).DeletedCount;
        }

        /// <summary>
        /// Asynchronously deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteManyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return (await HandlePartitioned<TDocument, TKey>(partitionKey).DeleteManyAsync(filter)).DeletedCount;
        }

        /// <summary>
        /// Asynchronously deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!documents.Any())
            {
                return 0;
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                long deleteCount = 0;
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    var groupIdsTodelete = group.Select(e => e.Id).ToArray();
                    deleteCount += (await HandlePartitioned<TDocument, TKey>(group.FirstOrDefault()).DeleteManyAsync(x => groupIdsTodelete.Contains(x.Id))).DeletedCount;
                }
                return deleteCount;
            }
            else
            {
                var idsTodelete = documents.Select(e => e.Id).ToArray();
                return (await HandlePartitioned<TDocument, TKey>(documents.FirstOrDefault()).DeleteManyAsync(x => idsTodelete.Contains(x.Id))).DeletedCount;
            }
        }

        /// <summary>
        /// Deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteMany<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!documents.Any())
            {
                return 0;
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                long deleteCount = 0;
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    var groupIdsTodelete = group.Select(e => e.Id).ToArray();
                    deleteCount += HandlePartitioned<TDocument, TKey>(group.FirstOrDefault()).DeleteMany(x => groupIdsTodelete.Contains(x.Id)).DeletedCount;
                }
                return deleteCount;
            }
            else
            {
                var idsTodelete = documents.Select(e => e.Id).ToArray();
                return HandlePartitioned<TDocument, TKey>(documents.FirstOrDefault()).DeleteMany(x => idsTodelete.Contains(x.Id)).DeletedCount;
            }
        }

        /// <summary>
        /// Deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteMany<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).DeleteMany(filter).DeletedCount;
        }

        #endregion

        #region Index
        /// <summary>
        /// Returns the names of the indexes present on a collection.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="partitionKey">An optional partition key</param>
        /// <returns>A list containing the names of the indexes on on the concerned collection.</returns>
        public async Task<List<string>> GetIndexesNamesAsync<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var indexCursor = await HandlePartitioned<TDocument, TKey>(partitionKey).Indexes.ListAsync();
            var indexes = await indexCursor.ToListAsync();
            return indexes.Select(e => e["name"].ToString()).ToList();
        }

        /// <summary>
        /// Create a text index on the given field.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateTextIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Indexes
                                                                   .CreateOneAsync(
                                                                     new CreateIndexModel<TDocument>(
                                                                     Builders<TDocument>.IndexKeys.Text(field),
                                                                     indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions)
                                                                   ));
        }

        /// <summary>
        /// Creates an index on the given field in ascending order.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateAscendingIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            var createOptions = indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions);
            var indexKey = Builders<TDocument>.IndexKeys;
            return await collection.Indexes
                                   .CreateOneAsync(
                new CreateIndexModel<TDocument>(indexKey.Ascending(field), createOptions));
        }

        /// <summary>
        /// Creates an index on the given field in descending order.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateDescendingIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            var createOptions = indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions);
            var indexKey = Builders<TDocument>.IndexKeys;
            return await collection.Indexes
                                   .CreateOneAsync(
                new CreateIndexModel<TDocument>(indexKey.Descending(field), createOptions));
        }

        /// <summary>
        /// Creates a hashed index on the given field.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateHashedIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            var createOptions = indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions);
            var indexKey = Builders<TDocument>.IndexKeys;
            return await collection.Indexes
                                   .CreateOneAsync(
                new CreateIndexModel<TDocument>(indexKey.Hashed(field), createOptions));
        }

        /// <summary>
        /// Creates a combined text index.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="fields">The fields we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateCombinedTextIndexAsync<TDocument, TKey>(IEnumerable<Expression<Func<TDocument, object>>> fields, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            var createOptions = indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions);
            var listOfDefs = new List<IndexKeysDefinition<TDocument>>();
            foreach (var field in fields)
            {
                listOfDefs.Add(Builders<TDocument>.IndexKeys.Text(field));
            }
            return await collection.Indexes
                                   .CreateOneAsync(new CreateIndexModel<TDocument>(Builders<TDocument>.IndexKeys.Combine(listOfDefs), createOptions));
        }

        /// <summary>
        /// Drops the index given a field name
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="indexName">The name of the index</param>
        /// <param name="partitionKey">An optional partition key</param>
        public async Task DropIndexAsync<TDocument, TKey>(string indexName, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            await HandlePartitioned<TDocument, TKey>(partitionKey).Indexes.DropOneAsync(indexName);
        }
        #endregion

        #region Read TKey

        /// <summary>
        /// Asynchronously returns one document given its id.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="id">The Id of the document you want to get.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<TDocument> GetByIdAsync<TDocument, TKey>(TKey id, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", id);
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Returns one document given its id.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="id">The Id of the document you want to get.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TDocument GetById<TDocument, TKey>(TKey id, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", id);
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously returns one document given filter definition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="findOption">A mongodb filter option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public Task<TDocument> GetOneAsync<TDocument, TKey>(FilterDefinition<TDocument> condition, FindOptions findOption = null,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(condition, findOption).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Returns one document given filter definition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="findOption">A mongodb filter option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TDocument GetOne<TDocument, TKey>(FilterDefinition<TDocument> condition, FindOptions findOption = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(condition, findOption).FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously returns one document given an expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<TDocument> GetOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Returns one document given an expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TDocument GetOne<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Returns a collection cursor.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public IFindFluent<TDocument, TDocument> GetCursor<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter);
        }

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="countOption">A mongodb counting option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<bool> AnyAsync<TDocument, TKey>(FilterDefinition<TDocument> condition, CountOptions countOption = null, string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var count = await HandlePartitioned<TDocument, TKey>(partitionKey).CountDocumentsAsync(condition, countOption, cancellationToken);
            return count > 0;
        }

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="countOption">A mongodb counting option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public bool Any<TDocument, TKey>(FilterDefinition<TDocument> condition, CountOptions countOption = null,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var count = HandlePartitioned<TDocument, TKey>(partitionKey).CountDocuments(condition, countOption);
            return count > 0;
        }

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<bool> AnyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var count = await HandlePartitioned<TDocument, TKey>(partitionKey).CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            return count > 0;
        }

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public bool Any<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var count = HandlePartitioned<TDocument, TKey>(partitionKey).CountDocuments(filter);
            return count > 0;
        }

        /// <summary>
        /// Asynchronously returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="findOption">A mongodb filter option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public Task<List<TDocument>> GetAllAsync<TDocument, TKey>(FilterDefinition<TDocument> condition,
            FindOptions findOption = null, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(condition, findOption).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="findOption">A mongodb filter option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public List<TDocument> GetAll<TDocument, TKey>(FilterDefinition<TDocument> condition, FindOptions findOption = null,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(condition, findOption).ToList();
        }

        /// <summary>
        /// Asynchronously returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<List<TDocument>> GetAllAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<List<TDocument>> GetAllAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<Guid>
        {
            return await HandlePartitioned<TDocument, Guid>(partitionKey).Find(filter).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public List<TDocument> GetAll<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).ToList();
        }

        /// <summary>
        /// Asynchronously counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="countOption">A mongodb counting option.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public Task<long> CountAsync<TDocument, TKey>(FilterDefinition<TDocument> condition, CountOptions countOption = null,
            string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).CountDocumentsAsync(condition, countOption, cancellationToken);
        }

        /// <summary>
        /// Counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="countOption">A mongodb counting option.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        public long Count<TDocument, TKey>(FilterDefinition<TDocument> condition, CountOptions countOption = null,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).CountDocuments(condition, countOption);
        }

        /// <summary>
        /// Asynchronously counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<long> CountAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        public long Count<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).CountDocuments();
        }

        #endregion

        #region Min / Max

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by descending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<TDocument> GetByMaxAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> maxValueSelector, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                                     .SortByDescending(maxValueSelector)
                                                                     .Limit(1)
                                                                     .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by descending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        public TDocument GetByMax<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                               .SortByDescending(maxValueSelector)
                                                               .Limit(1)
                                                               .FirstOrDefault();
        }

        /// <summary>
        /// Gets the document with the minimum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<TDocument> GetByMinAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> minValueSelector, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                                     .SortBy(minValueSelector)
                                                                     .Limit(1)
                                                                     .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the document with the minimum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        public TDocument GetByMin<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                               .SortBy(minValueSelector)
                                                               .Limit(1)
                                                               .FirstOrDefault();
        }

        /// <summary>
        /// Gets the maximum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the field for which you want the maximum value.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<TValue> GetMaxValueAsync<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetMaxMongoQuery<TDocument, TKey, TValue>(filter, maxValueSelector, partitionKey)
                                .Project(maxValueSelector)
                                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the maximum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        public TValue GetMaxValue<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetMaxMongoQuery<TDocument, TKey, TValue>(filter, maxValueSelector, partitionKey)
                      .Project(maxValueSelector)
                      .FirstOrDefault();
        }

        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<TValue> GetMinValueAsync<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetMinMongoQuery<TDocument, TKey, TValue>(filter, minValueSelector, partitionKey).Project(minValueSelector).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TValue GetMinValue<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetMinMongoQuery<TDocument, TKey, TValue>(filter, minValueSelector, partitionKey).Project(minValueSelector).FirstOrDefault();
        }


        #endregion Min / Max

        #region Sum TKey

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<int> SumByAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, int>> selector,
                                                       string partitionKey = null,
                                                       CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetQuery<TDocument, TKey>(filter, partitionKey).SumAsync(selector, cancellationToken);
        }

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public int SumBy<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, int>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetQuery<TDocument, TKey>(filter, partitionKey).Sum(selector);
        }

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<decimal> SumByAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetQuery<TDocument, TKey>(filter, partitionKey).SumAsync(selector, cancellationToken);
        }

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public decimal SumBy<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetQuery<TDocument, TKey>(filter, partitionKey).Sum(selector);
        }

        #endregion Sum TKey

        #region
        /// <summary>
        /// Groups a collection of documents given a grouping criteria, 
        /// and returns a dictionary of listed document groups with keys having the different values of the grouping criteria.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TGroupKey">The type of the grouping criteria.</typeparam>
        /// <typeparam name="TProjection">The type of the projected group.</typeparam>
        /// <param name="groupingCriteria">The grouping criteria.</param>
        /// <param name="groupProjection">The projected group result.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public List<TProjection> GroupBy<TDocument, TGroupKey, TProjection, TKey>(
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class, new()
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey)
                             .Aggregate()
                             .Group(groupingCriteria, groupProjection)
                             .ToList();

        }

        /// <summary>
        /// Groups filtered a collection of documents given a grouping criteria, 
        /// and returns a dictionary of listed document groups with keys having the different values of the grouping criteria.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TGroupKey">The type of the grouping criteria.</typeparam>
        /// <typeparam name="TProjection">The type of the projected group.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The grouping criteria.</param>
        /// <param name="projection">The projected group result.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public List<TProjection> GroupBy<TDocument, TGroupKey, TProjection, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TGroupKey>> selector,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> projection,
            string partitionKey = null)
                where TDocument : IDocument<TKey>
                where TKey : IEquatable<TKey>
                where TProjection : class, new()
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            return collection.Aggregate()
                             .Match(Builders<TDocument>.Filter.Where(filter))
                             .Group(selector, projection)
                             .ToList();
        }

        /// <summary>
        /// Groups filtered a collection of documents given a grouping criteria, 
        /// and returns a dictionary of listed document groups with keys having the different values of the grouping criteria.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TGroupKey">The type of the grouping criteria.</typeparam>
        /// <typeparam name="TProjection">The type of the projected group.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The grouping criteria.</param>
        /// <param name="projection">The projected group result.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<List<TProjection>> GroupByAsync<TDocument, TGroupKey, TProjection, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TGroupKey>> selector,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> projection,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
                where TDocument : IDocument<TKey>
                where TKey : IEquatable<TKey>
                where TProjection : class, new()
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            return await collection.Aggregate()
                             .Match(Builders<TDocument>.Filter.Where(filter))
                             .Group(selector, projection)
                             .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns a paginated list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="sortSelector">The property selector.</param>
        /// <param name="ascending">Order of the sorting.</param>
        /// <param name="skipNumber">The number of documents you want to skip. Default value is 0.</param>
        /// <param name="takeNumber">The number of documents you want to take. Default value is 50.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<List<TDocument>> GetSortedPaginatedAsync<TDocument, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, object>> sortSelector,
            bool ascending = true,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var sorting = ascending
                ? Builders<TDocument>.Sort.Ascending(sortSelector)
                : Builders<TDocument>.Sort.Descending(sortSelector);

            return await HandlePartitioned<TDocument, TKey>(partitionKey)
                                    .Find(filter)
                                    .Sort(sorting)
                                    .Skip(skipNumber)
                                    .Limit(takeNumber)
                                    .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns a paginated list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="sortDefinition">The sort definition.</param>
        /// <param name="skipNumber">The number of documents you want to skip. Default value is 0.</param>
        /// <param name="takeNumber">The number of documents you want to take. Default value is 50.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<List<TDocument>> GetSortedPaginatedAsync<TDocument, TKey>(
            Expression<Func<TDocument, bool>> filter,
            SortDefinition<TDocument> sortDefinition,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey)
                                    .Find(filter)
                                    .Sort(sortDefinition)
                                    .Skip(skipNumber)
                                    .Limit(takeNumber)
                                    .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns a projected document matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<TProjection> ProjectOneAsync<TDocument, TProjection, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TProjection>> projection,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter)
                                                                         .Project(projection)
                                                                         .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Returns a projected document matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TProjection ProjectOne<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter)
                                                                   .Project(projection)
                                                                   .FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously returns a list of projected documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TProjection>> projection,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter)
                                                                   .Project(projection)
                                                                   .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns a list of projected documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public List<TProjection> ProjectMany<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter)
                                                                   .Project(projection)
                                                                   .ToList();
        }
        #endregion

        #region Update
        /// <summary>
        /// Asynchronously Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument modifiedDocument)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", modifiedDocument.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(modifiedDocument).ReplaceOneAsync(filter, modifiedDocument);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        public bool UpdateOne<TDocument, TKey>(TDocument modifiedDocument)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", modifiedDocument.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(modifiedDocument).ReplaceOne(filter, modifiedDocument);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOneAsync(filter, update);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        public bool UpdateOne<TDocument, TKey>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOne(filter, update);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOneAsync(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        public bool UpdateOne<TDocument, TKey, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = await collection.UpdateOneAsync(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await UpdateOneAsync<TDocument, TKey, TField>(Builders<TDocument>.Filter.Where(filter), field, value, partitionKey);
        }

        /// <summary>
        /// Updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public bool UpdateOne<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = collection.UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public bool UpdateOne<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return UpdateOne<TDocument, TKey, TField>(Builders<TDocument>.Filter.Where(filter), field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public async Task<long> UpdateManyAsync<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await UpdateManyAsync<TDocument, TKey, TField>(Builders<TDocument>.Filter.Where(filter), field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = await collection.UpdateManyAsync(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount;
        }

        /// <summary>
        /// For the entities selected by the filter, apply the update definition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> update, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await UpdateManyAsync<TDocument, TKey>(Builders<TDocument>.Filter.Where(filter), update, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, apply the update definition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument, TKey>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = await collection.UpdateManyAsync(filter, updateDefinition);
            return updateRes.ModifiedCount;
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public long UpdateMany<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return UpdateMany<TDocument, TKey, TField>(Builders<TDocument>.Filter.Where(filter), field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = collection.UpdateMany(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount;
        }

        /// <summary>
        /// For the entities selected by the filter, apply the update definition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="UpdateDefinition">The update definition.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument, TKey>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> UpdateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = collection.UpdateMany(filter, UpdateDefinition);
            return updateRes.ModifiedCount;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> UpdateOneAsync<TDocument, TKey>(IClientSessionHandle session, TDocument modifiedDocument, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", modifiedDocument.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(modifiedDocument)
                    .ReplaceOneAsync(session, filter, modifiedDocument, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public bool UpdateOne<TDocument, TKey>(IClientSessionHandle session, TDocument modifiedDocument, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", modifiedDocument.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(modifiedDocument).ReplaceOne(session, filter, modifiedDocument, cancellationToken: cancellationToken);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="documentToModify">The document to modify.</param>
        /// <param name="update">The update definition.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> UpdateOneAsync<TDocument, TKey>(IClientSessionHandle session, TDocument documentToModify, UpdateDefinition<TDocument> update, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOneAsync(session, filter, update, null, cancellationToken).ConfigureAwait(false);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="documentToModify">The document to modify.</param>
        /// <param name="update">The update definition.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public bool UpdateOne<TDocument, TKey>(IClientSessionHandle session, TDocument documentToModify, UpdateDefinition<TDocument> update, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOne(session, filter, update, null, cancellationToken);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field to update.</typeparam>
        /// <param name="session">The client session.</param> 
        /// <param name="documentToModify">The document to modify.</param>
        /// <param name="field">The field to update.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(IClientSessionHandle session, TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(documentToModify)
                    .UpdateOneAsync(session, filter, Builders<TDocument>.Update.Set(field, value), cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field to update.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="documentToModify">The document to modify.</param>
        /// <param name="field">The field to update.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public bool UpdateOne<TDocument, TKey, TField>(IClientSessionHandle session, TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOne(session, filter, Builders<TDocument>.Update.Set(field, value), cancellationToken: cancellationToken);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field to update.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="filter">The filter for the update.</param>
        /// <param name="field">The field to update.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="partitionKey">The optional partition key.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(IClientSessionHandle session, FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = await collection.UpdateOneAsync(session, filter, Builders<TDocument>.Update.Set(field, value), cancellationToken: cancellationToken).ConfigureAwait(false);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field to update.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="filter">The filter for the update.</param>
        /// <param name="field">The field to update.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="partitionKey">The optional partition key.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public Task<bool> UpdateOneAsync<TDocument, TKey, TField>(IClientSessionHandle session, Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return UpdateOneAsync<TDocument, TKey, TField>(session, Builders<TDocument>.Filter.Where(filter), field, value, partitionKey, cancellationToken);
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field to update.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="filter">The filter for the update.</param>
        /// <param name="field">The field to update.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="partitionKey">The optional partition key.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public bool UpdateOne<TDocument, TKey, TField>(IClientSessionHandle session, FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = collection.UpdateOne(session, filter, Builders<TDocument>.Update.Set(field, value), cancellationToken: cancellationToken);
            return updateRes.ModifiedCount == 1;
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field to update.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="filter">The filter for the update.</param>
        /// <param name="field">The field to update.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="partitionKey">The optional partition key.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns></returns>
        public bool UpdateOne<TDocument, TKey, TField>(IClientSessionHandle session, Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return UpdateOne<TDocument, TKey, TField>(session, Builders<TDocument>.Filter.Where(filter), field, value, partitionKey, cancellationToken);
        }
        #endregion

        #region Method
        /// <summary>
        /// Sets the value of the document Id if it is not set already.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="document">The document.</param>
        protected void FormatDocument<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
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
        #endregion

        #endregion
    }
}
