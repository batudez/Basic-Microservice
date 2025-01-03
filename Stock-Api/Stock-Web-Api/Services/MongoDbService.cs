﻿using MongoDB.Driver;

namespace Stock_Web_Api.Services
{
	public class MongoDbService
	{
		readonly IMongoDatabase _database;
       
        public MongoDbService(IConfiguration configuration)
        {
            MongoClient client = new(configuration.GetConnectionString("MongoDB"));
            _database = client.GetDatabase("StockAPIDB");
        }
        public IMongoCollection<T> GetCollection<T>() => _database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());

    }
}
