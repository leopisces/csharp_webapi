using DataService.HostApi.Controllers.Base;
using DataService.Mongo.Models;
using DataService.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace DataService.HostApi.Controllers.Test
{
    /// <summary>
    /// MongoDb测试
    /// </summary>
    [ApiGroup(GroupVersion.ApiTest)]
    public class MongoDbController : BaseController
    {
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task AddAsync()
        {
            ////MongoClient
            //var client = new MongoClient("mongodb://zqz:zqz170170@47.97.101.104:27017/?authMechanism=SCRAM-SHA-256&authSource=lp");

            ////Get a Database
            //var database = client.GetDatabase("lp");

            ////Get a Collection
            //var collection = database.GetCollection<BsonDocument>("cs");

            ////Insert a Document
            //var document = new BsonDocument
            //{
            //    { "name", "MongoDB" },
            //    { "type", "Database" },
            //    { "count", 1 },
            //    { "info", new BsonDocument
            //        {
            //            { "x", 203 },
            //            { "y", 102 }
            //        }}
            //};
            //collection.InsertOne(document);

            await MongoRepository.AddOneAsync<Document, Guid>(new ATest
            {
                PartitionKey = "cs"
            });
        }



        /// <summary>
        /// 一个测试的类
        /// </summary>
        private class ATest : Document, IPartitionedDocument
        {
            /// <summary>
            /// 
            /// </summary>
            public string PartitionKey { get; set; }
        }
    }
}
