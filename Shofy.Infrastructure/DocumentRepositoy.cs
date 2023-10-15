using Shofy.Entities;
using Shofy.UseCases.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Infrastructure
{
    public class DocumentRepositoy : IDocumentRepository
    {
        private readonly List<Document> _documents;

        public DocumentRepositoy()
        {
            _documents = new List<Document>
            {
                new Document
                {
                    Id = Guid.NewGuid(),
                    Name = "20231014_ShofyProject_LegoAvatar",
                    ContentType = "image/jpeg",
                    Path = "StaticFiles/20231014_ShofyProject_LegoAvatar.jpg",
                    DocumentStatus = DocumentStatus.Ready
                }
            };
        }

        public async Task<IEnumerable<Document>> GetDocumentsAsync(DocumentStatus? status = null)
        {
            return await Task.FromResult(_documents.Where(x => !status.HasValue || x.DocumentStatus == status));
        }

        public async Task<Document> SaveAsync(Document document)
        {
            document.Id = Guid.NewGuid();
            _documents.Add(document);

            return await Task.FromResult(document);
        }

        public async Task UpdateAsync(Document document)
        {
            var doc = _documents.Find(d => d.Id == document.Id) ?? throw new KeyNotFoundException();

            doc.Name = document.Name;
            doc.ContentType = document.ContentType;
            doc.DocumentStatus = document.DocumentStatus;
            doc.Path = document.Path;
            doc.Error = document.Error;

            await Task.CompletedTask;
        }

        public async Task UpdateDocumentStatusAsync(Guid id, DocumentStatus status, string? error = null)
        {
            var document = _documents.Find(d => d.Id == id) ?? throw new KeyNotFoundException();
            document.DocumentStatus = status;
            document.Error = error;

            await UpdateAsync(document);
        }
    }
}
