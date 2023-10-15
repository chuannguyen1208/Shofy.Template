using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shofy.Entities;
using Shofy.UseCases.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure.MongoDb.Documents
{
    internal class DocumentRepository : IDocumentRepository
    {
        private readonly IMongoCollection<Document> _documentsCollection;

        public DocumentRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _documentsCollection = mongoDatabase.GetCollection<Document>("Documents");
        }

        public async Task<IEnumerable<Document>> GetDocumentsAsync(DocumentStatus? status = null)
        {
            var result = await _documentsCollection.Find(x => !status.HasValue || x.DocumentStatus == status).ToListAsync();
            return result;
        }

        public async Task<Document> SaveAsync(Document document)
        {
            await _documentsCollection.InsertOneAsync(document);
            return document;
        }

        public async Task UpdateAsync(Document document)
        {
            await _documentsCollection.ReplaceOneAsync(d => d.Id == document.Id, document);
        }

        public async Task UpdateDocumentStatusAsync(Guid id, DocumentStatus status, string? error = null)
        {
            var filter = Builders<Document>.Filter
                .Eq(d => d.Id, id);

            var update = Builders<Document>.Update
                .Set(d => d.DocumentStatus, status)
                .Set(d => d.Error, error);

            await _documentsCollection.UpdateOneAsync(filter, update);
        }
    }
}
