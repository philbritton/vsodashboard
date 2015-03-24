using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSOClientData.Entities;

namespace VSOClientData
{
    public class MongoHelper<TDocument> where TDocument : MongoEntity
    {
        public MongoCollection<TDocument> Collection { get; private set; }

        public MongoDatabase Database { get; private set; }

        /// <summary>
        /// Instatiates the Mongo Instance with Collections by Type
        /// </summary>
        /// <param name="collectionName">(optional) if not specified will use the Class name</param>
        public MongoHelper(string collectionName = null)
        {
            var con = new MongoConnectionStringBuilder(ConfigurationManager.ConnectionStrings["mongoDb"].ConnectionString);

            var client = new MongoClient(con.ConnectionString);
            var server = client.GetServer();

            Database = server.GetDatabase(con.DatabaseName);
            var typeName = (collectionName ?? typeof(TDocument).Name).ToLower();

            
                Collection = Database.GetCollection<TDocument>(typeName);

        }


        public TDocument FindById(ObjectId Id)
        {
            var query = Query.EQ("_Id", Id);
            return Collection.FindOne(query);
        }


        public void Dispose()
        {

        }
    }
}
