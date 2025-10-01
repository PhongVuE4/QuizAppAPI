using MongoDB.Driver;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Quiz_Infrastructure
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public IMongoCollection<Question> Questions => GetCollection<Question>("question");

        public MongoDBContext(IConfiguration config)
        {
            try
            {
                var connectionString = config["MongoDb:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentException("MongoDB connection string is missing or empty.");
                }

                var client = new MongoClient(connectionString);
                _database = client.GetDatabase(config["MongoDb:DatabaseName"]);
                // Kiểm tra kết nối bằng cách gọi một lệnh đơn giản
                var dbList = client.ListDatabaseNames().ToList();
            }
            catch (MongoAuthenticationException ex)
            {
                throw new Exception("Failed to authenticate with MongoDB. Check your username, password, or connection string (e.g., ensure authSource=admin is included).", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect to MongoDB.", ex);
            }
        }
    }
}
