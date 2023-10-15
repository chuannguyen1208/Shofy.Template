using Shofy.Entities;
using Shofy.UseCases.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure.EfCore.Documents
{
    internal class DocumentRepository : IDocumentRepository
    {
        public Task<IEnumerable<Document>> GetDocumentsAsync(DocumentStatus? status = null)
        {
            throw new NotImplementedException();
        }

        public Task<Document> SaveAsync(Document document)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Document document)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDocumentStatusAsync(Guid id, DocumentStatus status, string? error = null)
        {
            throw new NotImplementedException();
        }
    }
}
