using DataService.Mongo.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DataService.Mongo.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using DataService.Mongo.IRepository;

namespace DataService.Mongo.Temp
{
    /// <summary>
    /// The base Repository, it is meant to be inherited from by your custom custom MongoRepository implementation.
    /// Its constructor must be given a connection string and a database name.
    /// </summary>
    public class BaseMongoRepository1 : MongoDbContext1, IBaseMongoRepository1
    {
        /// <summary>
        /// The constructor taking a connection string and a database name.
        /// </summary>
        /// <param name="connectionString">The connection string of the MongoDb server.</param>
        /// <param name="databaseName">The name of the database against which you want to perform operations.</param>
        protected BaseMongoRepository1(IOptions<MongoConfig> options) : base(options)
        {
        }

        #region Index
        /// <summary>
        /// Returns the names of the indexes present on a collection.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="partitionKey">An optional partition key</param>
        /// <returns>A list containing the names of the indexes on on the concerned collection.</returns>
        public async Task<List<string>> GetIndexesNamesAsync<TDocument>(string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await GetIndexesNamesAsync<TDocument, Guid>(partitionKey);
        }

        /// <summary>
        /// Create a text index on the given field.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateTextIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await CreateTextIndexAsync<TDocument, Guid>(field, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Creates an index on the given field in ascending order.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateAscendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await CreateAscendingIndexAsync<TDocument, Guid>(field, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Creates an index on the given field in descending order.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateDescendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await CreateDescendingIndexAsync<TDocument, Guid>(field, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Creates a hashed index on the given field.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateHashedIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await CreateHashedIndexAsync<TDocument, Guid>(field, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Creates a combined text index.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="fields">The fields we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateCombinedTextIndexAsync<TDocument>(IEnumerable<Expression<Func<TDocument, object>>> fields, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await CreateCombinedTextIndexAsync<TDocument, Guid>(fields, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Drops the index given a field name
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="indexName">The name of the index</param>
        /// <param name="partitionKey">An optional partition key</param>
        public async Task DropIndexAsync<TDocument>(string indexName, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            await DropIndexAsync<TDocument, Guid>(indexName, partitionKey);
        }
        #endregion

        #region Delete

        /// <summary>
        /// Asynchronously deletes a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteOneAsync<TDocument>(TDocument document) where TDocument : IDocument<Guid>
        {
            return await DeleteOneAsync<TDocument, Guid>(document);
        }

        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteOne<TDocument>(TDocument document) where TDocument : IDocument<Guid>
        {
            return DeleteOne<TDocument, Guid>(document);
        }

        /// <summary>
        /// Deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return DeleteOne<TDocument, Guid>(filter, partitionKey);
        }

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await DeleteOneAsync<TDocument, Guid>(filter, partitionKey);
        }

        /// <summary>
        /// Asynchronously deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await DeleteManyAsync<TDocument, Guid>(filter, partitionKey);
        }

        /// <summary>
        /// Asynchronously deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteManyAsync<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument<Guid>
        {
            return await DeleteManyAsync<TDocument, Guid>(documents);
        }

        /// <summary>
        /// Deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument<Guid>
        {
            return DeleteMany<TDocument, Guid>(documents);
        }

        /// <summary>
        /// Deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return DeleteMany<TDocument, Guid>(filter, partitionKey);
        }

        #endregion Delete

        #region Create

        /// <summary>
        /// Asynchronously adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task AddOneAsync<TDocument>(TDocument document, CancellationToken cancellationToken = default)
            where TDocument : IDocument<Guid>
        {
            await AddOneAsync<TDocument, Guid>(document, cancellationToken);
        }

        /// <summary>
        /// Adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        public void AddOne<TDocument>(TDocument document) where TDocument : IDocument<Guid>
        {
            AddOne<TDocument, Guid>(document);
        }

        /// <summary>
        /// Asynchronously adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task AddManyAsync<TDocument>(IEnumerable<TDocument> documents, CancellationToken cancellationToken = default)
            where TDocument : IDocument<Guid>
        {
            await AddManyAsync<TDocument, Guid>(documents, cancellationToken);
        }

        /// <summary>
        /// Adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        public void AddMany<TDocument>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<Guid>
        {
            AddMany<TDocument, Guid>(documents);
        }
        #endregion

        #region Update

        /// <summary>
        /// Asynchronously Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        public async Task<bool> UpdateOneAsync<TDocument>(TDocument modifiedDocument) where TDocument : IDocument<Guid>
        {
            return await UpdateOneAsync<TDocument, Guid>(modifiedDocument);
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        public bool UpdateOne<TDocument>(TDocument modifiedDocument) where TDocument : IDocument<Guid>
        {
            return UpdateOne<TDocument, Guid>(modifiedDocument);
        }

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        public async Task<bool> UpdateOneAsync<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<Guid>
        {
            return await UpdateOneAsync<TDocument, Guid>(documentToModify, update);

        }

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        public bool UpdateOne<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<Guid>
        {
            return UpdateOne<TDocument, Guid>(documentToModify, update);
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        public bool UpdateOne<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<Guid>
        {
            return UpdateOne<TDocument, Guid, TField>(documentToModify, field, value);
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<Guid>
        {
            return await UpdateOneAsync<TDocument, Guid, TField>(documentToModify, field, value);
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public bool UpdateOne<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return UpdateOne<TDocument, Guid, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public bool UpdateOne<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return UpdateOne<TDocument, Guid, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await UpdateOneAsync<TDocument, Guid, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await UpdateOneAsync<TDocument, Guid, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await UpdateManyAsync<TDocument, Guid, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await UpdateManyAsync<TDocument, Guid, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await UpdateManyAsync<TDocument, Guid>(filter, updateDefinition, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return await UpdateManyAsync<TDocument, Guid>(filter, updateDefinition, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return UpdateMany<TDocument, Guid, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return UpdateMany<TDocument, Guid, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return UpdateMany<TDocument, Guid>(filter, updateDefinition, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<Guid>
        {
            return UpdateMany<TDocument, Guid>(filter, updateDefinition, partitionKey);
        }

        #endregion Update

        #region Update TKey
        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return UpdateMany<TDocument, TKey>(filter, updateDefinition, partitionKey);
        }
        #endregion Update

        /// <summary>
        /// Asynchronously returns a paginated list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="skipNumber">The number of documents you want to skip. Default value is 0.</param>
        /// <param name="takeNumber">The number of documents you want to take. Default value is 50.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<List<TDocument>> GetPaginatedAsync<TDocument>(
            Expression<Func<TDocument, bool>> filter,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null)
            where TDocument : IDocument
        {
            return await HandlePartitioned<TDocument>(partitionKey).Find(filter).Skip(skipNumber).Limit(takeNumber).ToListAsync();
        }

        /// <summary>
        /// Asynchronously returns a paginated list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="skipNumber">The number of documents you want to skip. Default value is 0.</param>
        /// <param name="takeNumber">The number of documents you want to take. Default value is 50.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<List<TDocument>> GetPaginatedAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, int skipNumber = 0, int takeNumber = 50, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).Skip(skipNumber).Limit(takeNumber).ToListAsync();
        }

        #region Find And Update

        /// <summary>
        /// GetAndUpdateOne with filter
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<TDocument> GetAndUpdateOne<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument> options) where TDocument : IDocument
        {
            return await GetCollection<TDocument>().FindOneAndUpdateAsync(filter, update, options);
        }

        /// <summary>
        /// GetAndUpdateOne with filter
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<TDocument> GetAndUpdateOne<TDocument, TKey>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument> options)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>().FindOneAndUpdateAsync(filter, update, options);
        }

        #endregion Find And Update

        /// <summary>
        /// Sets the value of the document Id if it is not set already.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="document">The document.</param>
        protected void FormatDocument<TDocument>(TDocument document) where TDocument : IDocument
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            if (document.Id == default)
            {
                document.Id = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="partitionKey">The collection partition key.</param>
        /// <returns></returns>
        protected IMongoCollection<TDocument> HandlePartitioned<TDocument>(string partitionKey)
            where TDocument : IDocument<Guid>
        {
            if (!string.IsNullOrEmpty(partitionKey))
            {
                return GetCollection<TDocument>(partitionKey);
            }
            return GetCollection<TDocument>();
        }

        #region Read Only

        #region Sum TKey
        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public async Task<decimal> SumByAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await SumByAsync<TDocument, TKey>(filter, selector, partitionKey);
        }
        #endregion Sum TKey

        #region Pagination

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
        public async Task<List<TDocument>> GetSortedPaginatedAsync<TDocument, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, object>> sortSelector,
            bool ascending = true,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null)
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
                                    .ToListAsync();
        }
        #endregion Pagination

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public IMongoCollection<TDocument> HandlePartitioned<TDocument>(TDocument document)
            where TDocument : IDocument<Guid>
        {
            return HandlePartitioned<TDocument, Guid>(document);
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="id"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        public async Task<TDocument> GetByIdAsync<TDocument>(Guid id, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await GetByIdAsync<TDocument>(id, partitionKey);
        }

        /// <summary>
        /// GetById
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="id"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        public TDocument GetById<TDocument>(Guid id, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return GetById<TDocument>(id, partitionKey);
        }

        /// <summary>
        /// GetOneAsync
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="filter"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TDocument> GetOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await GetOneAsync(filter, partitionKey);
        }

        public TDocument GetOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return GetOne(filter, partitionKey);
        }

        public IFindFluent<TDocument, TDocument> GetCursor<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return GetCursor(filter, partitionKey);
        }

        public async Task<bool> AnyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await AnyAsync(filter, partitionKey);
        }

        public bool Any<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return Any(filter, partitionKey);
        }

        public List<TDocument> GetAll<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return GetAll(filter, partitionKey);
        }

        public async Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await CountAsync(filter, partitionKey);
        }

        public long Count<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return Count(filter, partitionKey);
        }

        public async Task<TDocument> GetByMaxAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await GetByMaxAsync(filter, orderByDescending, partitionKey);
        }

        public TDocument GetByMax<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return GetByMax(filter, orderByDescending, partitionKey);
        }

        public async Task<TDocument> GetByMinAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await GetByMinAsync(filter, orderByAscending, partitionKey);
        }

        public TDocument GetByMin<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return GetByMin(filter, orderByAscending, partitionKey);
        }

        public async Task<TValue> GetMaxValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await GetMaxValueAsync(filter, maxValueSelector, partitionKey);
        }

        public TValue GetMaxValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return GetMaxValue(filter, maxValueSelector, partitionKey);
        }

        public async Task<TValue> GetMinValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await GetMinValueAsync(filter, minValueSelector, partitionKey);
        }

        public TValue GetMinValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return GetMinValue(filter, minValueSelector, partitionKey);
        }

        public async Task<int> SumByAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, int>> selector, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await SumByAsync(filter, selector, partitionKey);
        }

        public async Task<decimal> SumByAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, decimal>> selector, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await SumByAsync(filter, selector, partitionKey);
        }

        public int SumBy<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, int>> selector, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return SumBy(filter, selector, partitionKey);
        }

        public decimal SumBy<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, decimal>> selector, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return SumBy(filter, selector, partitionKey);
        }

        public async Task<TProjection> ProjectOneAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<Guid>
            where TProjection : class
        {
            return await ProjectOneAsync(filter, projection, partitionKey);
        }

        public TProjection ProjectOne<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<Guid>
            where TProjection : class
        {
            return ProjectOne(filter, projection, partitionKey);
        }

        public async Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<Guid>
            where TProjection : class
        {
            return await ProjectManyAsync(filter, projection, partitionKey);
        }

        public List<TProjection> ProjectMany<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<Guid>
            where TProjection : class
        {
            return ProjectMany(filter, projection, partitionKey);
        }

        public List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(Expression<Func<TDocument, TGroupKey>> groupingCriteria, Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection, string partitionKey = null)
            where TDocument : IDocument<Guid>
            where TProjection : class, new()
        {
            return GroupBy(groupingCriteria, groupProjection, partitionKey);
        }

        public List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TGroupKey>> groupingCriteria, Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection, string partitionKey = null)
            where TDocument : IDocument<Guid>
            where TProjection : class, new()
        {
            return GroupBy(filter, groupingCriteria, groupProjection, partitionKey);
        }

        public async Task<List<TDocument>> GetSortedPaginatedAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> sortSelector, bool ascending = true, int skipNumber = 0, int takeNumber = 50, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await GetSortedPaginatedAsync(filter, sortSelector, ascending, skipNumber, takeNumber, partitionKey);
        }

        public async Task<List<TDocument>> GetSortedPaginatedAsync<TDocument>(Expression<Func<TDocument, bool>> filter, SortDefinition<TDocument> sortDefinition, int skipNumber = 0, int takeNumber = 50, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await GetSortedPaginatedAsync(filter, sortDefinition, skipNumber, takeNumber, partitionKey);
        }

        public async Task<List<TDocument>> GetAllAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<Guid>
        {
            return await base.GetAllAsync(filter, partitionKey);
        }
        #endregion
    }

    /// <summary>
    /// The base Repository, it is meant to be inherited from by your custom custom MongoRepository implementation.
    /// Its constructor must be given a connection string and a database name.
    /// </summary>
    /// <typeparam name="TKey">The type of the document Id.</typeparam>
    public class BaseMongoRepository1<TKey> : MongoDbContext1, IBaseMongoRepository1<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The constructor taking a connection string and a database name.
        /// </summary>
        /// <param name="connectionString">The connection string of the MongoDb server.</param>
        /// <param name="databaseName">The name of the database against which you want to perform operations.</param>
        protected BaseMongoRepository1(IOptions<MongoConfig> options) : base(options)
        {
        }

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="partitionKey">The collection partition key.</param>
        /// <returns></returns>
        protected IMongoCollection<TDocument> HandlePartitioned<TDocument>(string partitionKey)
            where TDocument : IDocument<TKey>
        {
            if (!string.IsNullOrEmpty(partitionKey))
            {
                return GetCollection<TDocument>(partitionKey);
            }
            return GetCollection<TDocument>();
        }

        #region Index
        /// <summary>
        /// Returns the names of the indexes present on a collection.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="partitionKey">An optional partition key</param>
        /// <returns>A list containing the names of the indexes on on the concerned collection.</returns>
        public async Task<List<string>> GetIndexesNamesAsync<TDocument>(string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await GetIndexesNamesAsync<TDocument, TKey>(partitionKey);
        }

        /// <summary>
        /// Create a text index on the given field.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateTextIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await CreateTextIndexAsync<TDocument, TKey>(field, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Creates an index on the given field in ascending order.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateAscendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await CreateAscendingIndexAsync<TDocument, TKey>(field, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Creates an index on the given field in descending order.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateDescendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await CreateDescendingIndexAsync<TDocument, TKey>(field, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Creates a hashed index on the given field.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="field">The field we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateHashedIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await CreateHashedIndexAsync<TDocument, TKey>(field, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Creates a combined text index.
        /// IndexCreationOptions can be supplied to further specify 
        /// how the creation should be done.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="fields">The fields we want to index.</param>
        /// <param name="indexCreationOptions">Options for creating an index.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The result of the create index operation.</returns>
        public async Task<string> CreateCombinedTextIndexAsync<TDocument>(IEnumerable<Expression<Func<TDocument, object>>> fields, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await CreateCombinedTextIndexAsync<TDocument, TKey>(fields, indexCreationOptions, partitionKey);
        }

        /// <summary>
        /// Drops the index given a field name
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="indexName">The name of the index</param>
        /// <param name="partitionKey">An optional partition key</param>
        public async Task DropIndexAsync<TDocument>(string indexName, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            await DropIndexAsync<TDocument, TKey>(indexName, partitionKey);
        }
        #endregion

        #region Create
        /// <summary>
        /// Asynchronously adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task AddOneAsync<TDocument>(TDocument document, CancellationToken cancellationToken = default) where TDocument : IDocument<TKey>
        {
            await AddOneAsync<TDocument, TKey>(document, cancellationToken);
        }

        /// <summary>
        /// Adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        public void AddOne<TDocument>(TDocument document) where TDocument : IDocument<TKey>
        {
            AddOne<TDocument, TKey>(document);
        }

        /// <summary>
        /// Asynchronously adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        public async Task AddManyAsync<TDocument>(IEnumerable<TDocument> documents, CancellationToken cancellationToken = default) where TDocument : IDocument<TKey>
        {
            await AddManyAsync<TDocument, TKey>(documents, cancellationToken);
        }

        /// <summary>
        /// Adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        public void AddMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument<TKey>
        {
            AddMany<TDocument, TKey>(documents);
        }
        #endregion

        #region Update

        /// <summary>
        /// Asynchronously Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        public async Task<bool> UpdateOneAsync<TDocument>(TDocument modifiedDocument) where TDocument : IDocument<TKey>
        {
            return await UpdateOneAsync<TDocument, TKey>(modifiedDocument);
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        public bool UpdateOne<TDocument>(TDocument modifiedDocument) where TDocument : IDocument<TKey>
        {
            return UpdateOne<TDocument, TKey>(modifiedDocument);
        }

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        public async Task<bool> UpdateOneAsync<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
        {
            return await UpdateOneAsync<TDocument, TKey>(documentToModify, update);
        }

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        public bool UpdateOne<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
        {
            return UpdateOne<TDocument, TKey>(documentToModify, update);
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        public bool UpdateOne<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
        {
            return UpdateOne<TDocument, TKey, TField>(documentToModify, field, value);
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
        {
            return await UpdateOneAsync<TDocument, TKey, TField>(documentToModify, field, value);
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public bool UpdateOne<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return UpdateOne<TDocument, TKey, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public bool UpdateOne<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return UpdateOne<TDocument, TKey, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await UpdateOneAsync<TDocument, TKey, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public async Task<bool> UpdateOneAsync<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await UpdateOneAsync<TDocument, TKey, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public async Task<long> UpdateManyAsync<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await UpdateManyAsync<TDocument, TKey, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await UpdateManyAsync<TDocument, TKey, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await UpdateManyAsync<TDocument, TKey>(filter, updateDefinition, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public async Task<long> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await UpdateManyAsync<TDocument, TKey>(filter, updateDefinition, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        public long UpdateMany<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return UpdateMany<TDocument, TKey, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return UpdateMany<TDocument, TKey, TField>(filter, field, value, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return UpdateMany<TDocument, TKey>(filter, updateDefinition, partitionKey);
        }

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        public long UpdateMany<TDocument>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return UpdateMany<TDocument, TKey>(filter, updateDefinition, partitionKey);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteOne<TDocument>(TDocument document)
            where TDocument : IDocument<TKey>
        {
            return DeleteOne<TDocument, TKey>(document);
        }

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteOneAsync<TDocument>(TDocument document)
            where TDocument : IDocument<TKey>
        {
            return await DeleteOneAsync<TDocument, TKey>(document);
        }

        /// <summary>
        /// Deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return DeleteOne<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await DeleteOneAsync<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Asynchronously deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await DeleteManyAsync<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Asynchronously deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public async Task<long> DeleteManyAsync<TDocument>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
        {
            return await DeleteManyAsync<TDocument, TKey>(documents);
        }

        /// <summary>
        /// Deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteMany<TDocument>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
        {
            return DeleteMany<TDocument, TKey>(documents);
        }

        /// <summary>
        /// Deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        public long DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return DeleteMany<TDocument, TKey>(filter, partitionKey);
        }
        #endregion

        #region Read Only
        #region Read

        /// <summary>
        /// Asynchronously returns one document given its id.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="id">The Id of the document you want to get.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<TDocument> GetByIdAsync<TDocument>(TKey id, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return await GetByIdAsync<TDocument, TKey>(id, partitionKey);
        }

        /// <summary>
        /// Returns one document given its id.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="id">The Id of the document you want to get.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TDocument GetById<TDocument>(TKey id, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return GetById<TDocument, TKey>(id, partitionKey);
        }

        /// <summary>
        /// Asynchronously returns one document given an expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<TDocument> GetOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return await GetOneAsync<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Returns one document given an expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TDocument GetOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return GetOne<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Returns a collection cursor.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public IFindFluent<TDocument, TDocument> GetCursor<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return GetCursor<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<bool> AnyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return await AnyAsync<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public bool Any<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return Any<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Asynchronously returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<List<TDocument>> GetAllAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return await GetAllAsync<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public List<TDocument> GetAll<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return GetAll<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Asynchronously counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        public async Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return await CountAsync<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        public long Count<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>
        {
            return Count<TDocument, TKey>(filter, partitionKey);
        }

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by descending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        public async Task<TDocument> GetByMaxAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await GetByMaxAsync<TDocument, TKey>(filter, maxValueSelector, partitionKey);
        }

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by descending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        /// <returns></returns>
        public TDocument GetByMax<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return GetByMax<TDocument, TKey>(filter, maxValueSelector, partitionKey);
        }

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        public async Task<TDocument> GetByMinAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await GetByMinAsync<TDocument, TKey>(filter, minValueSelector, partitionKey);
        }

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        public TDocument GetByMin<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return GetByMin<TDocument, TKey>(filter, minValueSelector, partitionKey);
        }

        /// <summary>
        /// Gets the maximum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        public async Task<TValue> GetMaxValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await GetMaxValueAsync<TDocument, TKey, TValue>(filter, maxValueSelector, partitionKey);
        }

        /// <summary>
        /// Gets the maximum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        public TValue GetMaxValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return GetMaxValue<TDocument, TKey, TValue>(filter, maxValueSelector, partitionKey);
        }

        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<TValue> GetMinValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await GetMinValueAsync<TDocument, TKey, TValue>(filter, minValueSelector, partitionKey);
        }

        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TValue GetMinValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return GetMinValue<TDocument, TKey, TValue>(filter, minValueSelector, partitionKey);
        }

        #endregion

        #region Maths

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public async Task<int> SumByAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, int>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await SumByAsync<TDocument, TKey>(filter, selector, partitionKey);
        }

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public async Task<decimal> SumByAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await SumByAsync<TDocument, TKey>(filter, selector, partitionKey);
        }

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public int SumBy<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, int>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return SumBy<TDocument, TKey>(filter, selector, partitionKey);
        }

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public decimal SumBy<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return SumBy<TDocument, TKey>(filter, selector, partitionKey);
        }

        #endregion Maths

        #region Project

        /// <summary>
        /// Asynchronously returns a projected document matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<TProjection> ProjectOneAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class
        {
            return await ProjectOneAsync<TDocument, TProjection, TKey>(filter, projection, partitionKey);
        }

        /// <summary>
        /// Returns a projected document matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public TProjection ProjectOne<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class
        {
            return ProjectOne<TDocument, TProjection, TKey>(filter, projection, partitionKey);
        }

        /// <summary>
        /// Asynchronously returns a list of projected documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class
        {
            return await ProjectManyAsync<TDocument, TProjection, TKey>(filter, projection, partitionKey);
        }

        /// <summary>
        /// Asynchronously returns a list of projected documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public List<TProjection> ProjectMany<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class
        {
            return ProjectMany<TDocument, TProjection, TKey>(filter, projection, partitionKey);
        }


        #endregion Project

        /// <summary>
        /// Groups a collection of documents given a grouping criteria, 
        /// and returns a dictionary of listed document groups with keys having the different values of the grouping criteria.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TGroupKey">The type of the grouping criteria.</typeparam>
        /// <typeparam name="TProjection">The type of the projected group.</typeparam>
        /// <param name="groupingCriteria">The grouping criteria.</param>
        /// <param name="groupProjection">The projected group result.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class, new()
        {
            return GroupBy<TDocument, TGroupKey, TProjection, TKey>(groupingCriteria, groupProjection, partitionKey);
        }

        /// <summary>
        /// Groups filtered a collection of documents given a grouping criteria, 
        /// and returns a dictionary of listed document groups with keys having the different values of the grouping criteria.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TGroupKey">The type of the grouping criteria.</typeparam>
        /// <typeparam name="TProjection">The type of the projected group.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="groupingCriteria">The grouping criteria.</param>
        /// <param name="groupProjection">The projected group result.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        public List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
                where TDocument : IDocument<TKey>
                where TProjection : class, new()
        {
            return GroupBy<TDocument, TGroupKey, TProjection, TKey>(filter, groupingCriteria, groupProjection, partitionKey);
        }

        /// <summary>
        /// Asynchronously returns a paginated list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="sortSelector">The property selector.</param>
        /// <param name="ascending">Order of the sorting.</param>
        /// <param name="skipNumber">The number of documents you want to skip. Default value is 0.</param>
        /// <param name="takeNumber">The number of documents you want to take. Default value is 50.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<List<TDocument>> GetSortedPaginatedAsync<TDocument>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, object>> sortSelector,
            bool ascending = true,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await GetSortedPaginatedAsync<TDocument, TKey>(filter, sortSelector, ascending, skipNumber, takeNumber, partitionKey);
        }

        /// <summary>
        /// Asynchronously returns a paginated list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="sortDefinition">The sort definition.</param>
        /// <param name="skipNumber">The number of documents you want to skip. Default value is 0.</param>
        /// <param name="takeNumber">The number of documents you want to take. Default value is 50.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public async Task<List<TDocument>> GetSortedPaginatedAsync<TDocument>(
            Expression<Func<TDocument, bool>> filter,
            SortDefinition<TDocument> sortDefinition,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
        {
            return await GetSortedPaginatedAsync<TDocument, TKey>(filter, sortDefinition, skipNumber, takeNumber, partitionKey);
        }

        public Task<decimal> SumByAsync<TDocument, TKey1>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, decimal>> selector, string partitionKey = null)
            where TDocument : IDocument<TKey1>
            where TKey1 : IEquatable<TKey1>
        {
            //TODO
            throw new NotImplementedException();
        }

        public Task<List<TDocument>> GetSortedPaginatedAsync<TDocument, TKey1>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> sortSelector, bool ascending = true, int skipNumber = 0, int takeNumber = 50, string partitionKey = null)
            where TDocument : IDocument<TKey1>
            where TKey1 : IEquatable<TKey1>
        {
            //TODO
            throw new NotImplementedException();
        }
        #endregion
    }
}
