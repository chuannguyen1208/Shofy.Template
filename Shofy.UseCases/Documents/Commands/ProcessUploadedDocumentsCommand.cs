using MediatR;
using Shofy.Entities;
using Shofy.UseCases.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.Documents.Commands
{
    public record ProcessUploadedDocumentsCommand : IRequest
    {
        internal class ProcessUploadedDocumentsCommandHandler : IRequestHandler<ProcessUploadedDocumentsCommand>
        {
            private readonly IDocumentRepository _documentRepository;
            private readonly IDocumentAdapter _documentAdapter;

            public ProcessUploadedDocumentsCommandHandler(
                IDocumentRepository documentRepository, 
                IDocumentAdapter documentAdapter)
            {
                _documentRepository = documentRepository;
                _documentAdapter = documentAdapter;
            }

            public async Task Handle(ProcessUploadedDocumentsCommand request, CancellationToken cancellationToken)
            {
                var documents = await _documentRepository.GetDocumentsAsync(status: DocumentStatus.New);

                foreach (var document in documents)
                {
                    try
                    {
                        await _documentRepository.UpdateDocumentStatusAsync(document.Id, DocumentStatus.Processing);

                        string resizedPath = await _documentAdapter.ResizeAsync(
                            documentPath: document.Path,
                            width: document.GetRecommendedWidth());

                        document.Path = resizedPath;
                        document.DocumentStatus = DocumentStatus.Ready;

                        await _documentRepository.UpdateAsync(document);
                    }
                    catch (Exception ex)
                    {
                        await _documentRepository
                            .UpdateDocumentStatusAsync(document.Id, DocumentStatus.Error, ex.Message);
                    }

                }
            }
        }
    }
}
