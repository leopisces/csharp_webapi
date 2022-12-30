using DataService.Mongo.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataService.Mongo.Temp
{
    /// <summary>
    /// The interface exposing all the CRUD and Index functionalities for Key typed repositories.
    /// </summary>
    /// <typeparam name="TKey">The type of the document Id.</typeparam>
    public interface IBaseMongoRepository1<TKey> where TKey : IEquatable<TKey>
    {
        #region Index
        /// <summary>
        /// Returns the names of the indexes present on a collection.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="partitionKey">An optional partition key</param>
        /// <returns>A list containing the names of the indexes on on the concerned collection.</returns>
        Task<List<string>> GetIndexesNamesAsync<TDocument>(string partitionKey = null)
            where TDocument : IDocument<TKey>;

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
        Task<string> CreateTextIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>;

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
        Task<string> CreateAscendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>;

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
        Task<string> CreateDescendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>;

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
        Task<string> CreateHashedIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>;

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
        Task<string> CreateCombinedTextIndexAsync<TDocument>(IEnumerable<Expression<Func<TDocument, object>>> fields, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Drops the index given a field name
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="indexName">The name of the index</param>
        /// <param name="partitionKey">An optional partition key</param>
        Task DropIndexAsync<TDocument>(string indexName, string partitionKey = null)
            where TDocument : IDocument<TKey>;
        #endregion

        #region Create
        /// <summary>
        /// Asynchronously adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task AddOneAsync<TDocument>(TDocument document, CancellationToken cancellationToken = default) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        void AddOne<TDocument>(TDocument document) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task AddManyAsync<TDocument>(IEnumerable<TDocument> documents, CancellationToken cancellationToken = default) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        void AddMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument<TKey>;
        #endregion

        #region Update
        /// <summary>
        /// Asynchronously Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        Task<bool> UpdateOneAsync<TDocument>(TDocument modifiedDocument) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        bool UpdateOne<TDocument>(TDocument modifiedDocument) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        Task<bool> UpdateOneAsync<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        bool UpdateOne<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        bool UpdateOne<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        Task<bool> UpdateOneAsync<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        bool UpdateOne<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        bool UpdateOne<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        Task<bool> UpdateOneAsync<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        Task<bool> UpdateOneAsync<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        Task<long> UpdateManyAsync<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        Task<long> UpdateManyAsync<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        Task<long> UpdateManyAsync<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        Task<long> UpdateManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        long UpdateMany<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entities selected by the filter, updates the property field with the given value.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        long UpdateMany<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        long UpdateMany<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        long UpdateMany<TDocument>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>;
        #endregion

        #region Delete
        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        long DeleteOne<TDocument>(TDocument document)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteOneAsync<TDocument>(TDocument document)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        long DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteManyAsync<TDocument>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        long DeleteMany<TDocument>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        long DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>;
        #endregion

        #region Read Only
        #region Read

        /// <summary>
        /// Asynchronously returns one document given its id.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="id">The Id of the document you want to get.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<TDocument> GetByIdAsync<TDocument>(TKey id, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Returns one document given its id.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="id">The Id of the document you want to get.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TDocument GetById<TDocument>(TKey id, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously returns one document given an expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<TDocument> GetOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Returns one document given an expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TDocument GetOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Returns a collection cursor.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        IFindFluent<TDocument, TDocument> GetCursor<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<bool> AnyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        bool Any<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<List<TDocument>> GetAllAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        List<TDocument> GetAll<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Asynchronously counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        /// <summary>
        /// Counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        long Count<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument<TKey>;

        #endregion

        #region Min / Max

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="orderByDescending">A property selector to order by descending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        Task<TDocument> GetByMaxAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="orderByDescending">A property selector to order by descending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        /// <returns></returns>
        TDocument GetByMax<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="orderByAscending">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        Task<TDocument> GetByMinAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="orderByAscending">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        TDocument GetByMin<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Gets the maximum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        Task<TValue> GetMaxValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Gets the maximum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        TValue GetMaxValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<TValue> GetMinValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TValue GetMinValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>;

        #endregion

        #region Maths

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        Task<int> SumByAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, int>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        Task<decimal> SumByAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        int SumBy<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, int>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>;

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        decimal SumBy<TDocument>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>;

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
        Task<TProjection> ProjectOneAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class;

        /// <summary>
        /// Returns a projected document matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TProjection ProjectOne<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class;

        /// <summary>
        /// Asynchronously returns a list of projected documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class;

        /// <summary>
        /// Asynchronously returns a list of projected documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        List<TProjection> ProjectMany<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class;

        #endregion Project

        #region Group By

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
        List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TProjection : class, new();

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
        List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
                where TDocument : IDocument<TKey>
                where TProjection : class, new();

        #endregion Group By

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
        Task<List<TDocument>> GetSortedPaginatedAsync<TDocument>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, object>> sortSelector,
            bool ascending = true,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null)
            where TDocument : IDocument<TKey>;

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
        Task<List<TDocument>> GetSortedPaginatedAsync<TDocument>(
            Expression<Func<TDocument, bool>> filter,
            SortDefinition<TDocument> sortDefinition,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null)
            where TDocument : IDocument<TKey>;

        #endregion Pagination
        #endregion
    }


    /// <summary>
    /// The IBaseMongoRepository interface exposes the CRUD functionality of the BaseMongoRepository.
    /// </summary>
    public interface IBaseMongoRepository1 : IBaseMongoRepository1<Guid>
    {
        #region Index
        /// <summary>
        /// Returns the names of the indexes present on a collection.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="partitionKey">An optional partition key</param>
        /// <returns>A list containing the names of the indexes on on the concerned collection.</returns>
        Task<List<string>> GetIndexesNamesAsync<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<string> CreateTextIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<string> CreateAscendingIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<string> CreateDescendingIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
        where TDocument : IDocument<TKey>
        where TKey : IEquatable<TKey>;

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
        Task<string> CreateHashedIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<string> CreateCombinedTextIndexAsync<TDocument, TKey>(IEnumerable<Expression<Func<TDocument, object>>> fields, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Drops the index given a field name
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="indexName">The name of the index</param>
        /// <param name="partitionKey">An optional partition key</param>
        Task DropIndexAsync<TDocument, TKey>(string indexName, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;
        #endregion

        #region Create
        /// <summary>
        /// Asynchronously adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task AddOneAsync<TDocument, TKey>(TDocument document, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Adds a document to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="document">The document you want to add.</param>
        void AddOne<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task AddManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Adds a list of documents to the collection.
        /// Populates the Id and AddedAtUtc fields if necessary.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documents">The documents you want to add.</param>
        void AddMany<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;
        #endregion

        #region Update
        /// <summary>
        /// Asynchronously Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument modifiedDocument)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        bool UpdateOne<TDocument, TKey>(TDocument modifiedDocument)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Takes a document you want to modify and applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="update">The update definition for the document.</param>
        bool UpdateOne<TDocument, TKey>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// For the entity selected by the filter, updates the property field with the given value..
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        /// <param name="partitionKey">The partition key for the document.</param>
        bool UpdateOne<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Updates the property field with the given value update a property field in entities.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="documentToModify">The document you want to modify.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of the property field.</param>
        bool UpdateOne<TDocument, TKey, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        bool UpdateOne<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<long> UpdateManyAsync<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<long> UpdateManyAsync<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        Task<long> UpdateManyAsync<TDocument, TKey>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        Task<long> UpdateManyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        long UpdateMany<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        long UpdateMany<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        long UpdateMany<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// For the entities selected by the filter, applies the update you have defined in MongoDb.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">The document filter.</param>
        /// <param name="updateDefinition">The update definition to apply.</param>
        /// <param name="partitionKey">The value of the partition key.</param>
        long UpdateMany<TDocument, TKey>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> updateDefinition, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;
        #endregion

        #region Delete
        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        long DeleteOne<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="document">The document you want to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteOneAsync<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        long DeleteOne<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously deletes a document matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteManyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Deletes a list of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="documents">The list of documents to delete.</param>
        /// <returns>The number of documents deleted.</returns>
        long DeleteMany<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Deletes the documents matching the condition of the LINQ expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>The number of documents deleted.</returns>
        long DeleteMany<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;
        #endregion

        /// <summary>
        /// Asynchronously returns a paginated list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter"></param>
        /// <param name="skipNumber">The number of documents you want to skip. Default value is 0.</param>
        /// <param name="takeNumber">The number of documents you want to take. Default value is 50.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<List<TDocument>> GetPaginatedAsync<TDocument>(Expression<Func<TDocument, bool>> filter, int skipNumber = 0, int takeNumber = 50, string partitionKey = null)
            where TDocument : IDocument;

        /// <summary>
        /// Asynchronously returns a paginated list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter"></param>
        /// <param name="skipNumber">The number of documents you want to skip. Default value is 0.</param>
        /// <param name="takeNumber">The number of documents you want to take. Default value is 50.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        Task<List<TDocument>> GetPaginatedAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, int skipNumber = 0, int takeNumber = 50, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// GetAndUpdateOne with filter
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<TDocument> GetAndUpdateOne<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument> options)
            where TDocument : IDocument;

        /// <summary>
        /// GetAndUpdateOne with filter
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<TDocument> GetAndUpdateOne<TDocument, TKey>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument> options)
                                        where TDocument : IDocument<TKey>
                                        where TKey : IEquatable<TKey>;

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
        /// <returns>A boolean value indicating success.</returns>
        bool UpdateOne<TDocument, TKey, TField>(IClientSessionHandle session, Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        /// <returns>A boolean value indicating success.</returns>
        bool UpdateOne<TDocument, TKey, TField>(IClientSessionHandle session, FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        /// <returns>A boolean value indicating success.</returns>
        bool UpdateOne<TDocument, TKey, TField>(IClientSessionHandle session, TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>A boolean value indicating success.</returns>
        bool UpdateOne<TDocument, TKey>(IClientSessionHandle session, TDocument modifiedDocument, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="documentToModify">The document to modify.</param>
        /// <param name="update">The update definition.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>A boolean value indicating success.</returns>
        bool UpdateOne<TDocument, TKey>(IClientSessionHandle session, TDocument documentToModify, UpdateDefinition<TDocument> update, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        /// <returns>A boolean value indicating success.</returns>
        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(IClientSessionHandle session, Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(IClientSessionHandle session, FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        /// <returns>A boolean value indicating success.</returns>
        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(IClientSessionHandle session, TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="modifiedDocument">The document with the modifications you want to persist.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>A boolean value indicating success.</returns>
        Task<bool> UpdateOneAsync<TDocument, TKey>(IClientSessionHandle session, TDocument modifiedDocument, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="session">The client session.</param>
        /// <param name="documentToModify">The document to modify.</param>
        /// <param name="update">The update definition.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>A boolean value indicating success.</returns>
        Task<bool> UpdateOneAsync<TDocument, TKey>(IClientSessionHandle session, TDocument documentToModify, UpdateDefinition<TDocument> update, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #region Read TKey

        /// <summary>
        /// Asynchronously returns one document given its id.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="id">The Id of the document you want to get.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<TDocument> GetByIdAsync<TDocument, TKey>(TKey id, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns one document given its id.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="id">The Id of the document you want to get.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TDocument GetById<TDocument, TKey>(TKey id, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously returns one document given filter definition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="findOption">A mongodb filter option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<TDocument> GetOneAsync<TDocument, TKey>(FilterDefinition<TDocument> condition, FindOptions findOption = null,
            string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns one document given filter definition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="findOption">A mongodb filter option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TDocument GetOne<TDocument, TKey>(FilterDefinition<TDocument> condition, FindOptions findOption = null, string partitionKey = null)
            where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously returns one document given an expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<TDocument> GetOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns one document given an expression filter.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TDocument GetOne<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns a collection cursor.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        IFindFluent<TDocument, TDocument> GetCursor<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="countOption">A mongodb counting option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<bool> AnyAsync<TDocument, TKey>(FilterDefinition<TDocument> condition, CountOptions countOption = null, string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="countOption">A mongodb counting option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        bool Any<TDocument, TKey>(FilterDefinition<TDocument> condition, CountOptions countOption = null, string partitionKey = null)
            where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<bool> AnyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns true if any of the document of the collection matches the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        bool Any<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="findOption">A mongodb filter option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<List<TDocument>> GetAllAsync<TDocument, TKey>(FilterDefinition<TDocument> condition,
            FindOptions findOption = null, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="findOption">A mongodb filter option.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        List<TDocument> GetAll<TDocument, TKey>(FilterDefinition<TDocument> condition, FindOptions findOption = null, string partitionKey = null)
            where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<List<TDocument>> GetAllAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Returns a list of the documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        List<TDocument> GetAll<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="countOption">A mongodb counting option.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<long> CountAsync<TDocument, TKey>(FilterDefinition<TDocument> condition, CountOptions countOption = null,
            string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="condition">A mongodb filter definition.</param>
        /// <param name="countOption">A mongodb counting option.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        long Count<TDocument, TKey>(FilterDefinition<TDocument> condition, CountOptions countOption = null,
            string partitionKey = null)
            where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Asynchronously counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<long> CountAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Counts how many documents match the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partitionKey</param>
        long Count<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<TDocument> GetByMaxAsync<TDocument, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, object>> maxValueSelector,
            string partitionKey = null, CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Gets the document with the maximum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="orderByDescending">A property selector to order by descending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        TDocument GetByMax<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Gets the document with the minimum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector for the minimum value you are looking for.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<TDocument> GetByMinAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, object>> minValueSelector,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Gets the document with the minimum value of a specified property in a MongoDB collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector for the minimum value you are looking for.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        TDocument GetByMin<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Gets the maximum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector for the maximum value you are looking for.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<TValue> GetMaxValueAsync<TDocument, TKey, TValue>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TValue>> maxValueSelector,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Gets the maximum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="maxValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partitionKey.</param>
        TValue GetMaxValue<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<TValue> GetMinValueAsync<TDocument, TKey, TValue>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TValue>> minValueSelector,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Gets the minimum value of a property in a mongodb collections that is satisfying the filter.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <typeparam name="TValue">The type of the value used to order the query.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="minValueSelector">A property selector to order by ascending.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TValue GetMinValue<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Sum

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        /// <param name="cancellationToken">An optional cancellation Token.</param>
        Task<int> SumByAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, int>> selector,
                                                       string partitionKey = null,
                                                       CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        int SumBy<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, int>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        Task<decimal> SumByAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Sums the values of a selected field for a given filtered collection of documents.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="selector">The field you want to sum.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        decimal SumBy<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, decimal>> selector,
                                                       string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion Sum

        #region Project TKey

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
        Task<TProjection> ProjectOneAsync<TDocument, TProjection, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TProjection>> projection,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class;

        /// <summary>
        /// Returns a projected document matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter"></param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        TProjection ProjectOne<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
                                        where TDocument : IDocument<TKey>
                                        where TKey : IEquatable<TKey>
                                        where TProjection : class;

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
        Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TProjection>> projection,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class;

        /// <summary>
        /// Asynchronously returns a list of projected documents matching the filter condition.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <typeparam name="TProjection">The type representing the model you want to project to.</typeparam>
        /// <param name="filter"></param>
        /// <param name="projection">The projection expression.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        List<TProjection> ProjectMany<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class;

        #endregion

        #region Group By

        /// <summary>
        /// Groups filtered a collection of documents given a grouping criteria, 
        /// and returns a dictionary of listed document groups with keys having the different values of the grouping criteria.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TGroupKey">The type of the grouping criteria.</typeparam>
        /// <typeparam name="TProjection">The type of the projected group.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="groupingCriteria">The grouping criteria.</param>
        /// <param name="groupProjection">The projected group result.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        List<TProjection> GroupBy<TDocument, TGroupKey, TProjection, TKey>(
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class, new();

        /// <summary>
        /// Groups filtered a collection of documents given a grouping criteria, 
        /// and returns a dictionary of listed document groups with keys having the different values of the grouping criteria.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TGroupKey">The type of the grouping criteria.</typeparam>
        /// <typeparam name="TProjection">The type of the projected group.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="groupingCriteria">The grouping criteria.</param>
        /// <param name="groupProjection">The projected group result.</param>
        /// <param name="partitionKey">The partition key of your document, if any.</param>
        List<TProjection> GroupBy<TDocument, TGroupKey, TProjection, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
                where TDocument : IDocument<TKey>
                where TKey : IEquatable<TKey>
                where TProjection : class, new();

        #endregion Group By

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
        Task<List<TDocument>> GetSortedPaginatedAsync<TDocument, TKey>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, object>> sortSelector,
            bool ascending = true,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

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
        Task<List<TDocument>> GetSortedPaginatedAsync<TDocument, TKey>(
            Expression<Func<TDocument, bool>> filter,
            SortDefinition<TDocument> sortDefinition,
            int skipNumber = 0,
            int takeNumber = 50,
            string partitionKey = null,
            CancellationToken cancellationToken = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion Pagination
    }

}
