using Shofy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.Files
{
    public interface IDocumentRepository
    {
        Task<Document> SaveAsync(Document document);
        Task<IEnumerable<Document>> GetDocumentsAsync(DocumentStatus? status = null);
        Task UpdateDocumentStatusAsync(Guid id, DocumentStatus status, string? error = null);
        Task UpdateAsync(Document document);
    }
}
