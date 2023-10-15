using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shofy.Entities;
using Shofy.UseCases.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.Documents.Commands
{
    public record UploadDocumentCommand(IFormFile File) : IRequest<DocumentDTO>
    {
        internal class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, DocumentDTO>
        {
            private readonly IConfiguration _configuration;
            private readonly IDocumentRepository _documentRepository;
            private readonly IMapper _mapper;

            public UploadDocumentCommandHandler(
                IConfiguration configuration, 
                IDocumentRepository documentRepository, 
                IMapper mapper)
            {
                _configuration = configuration;
                _documentRepository = documentRepository;
                _mapper = mapper;
            }

            public async Task<DocumentDTO> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
            {
                var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}-Shofy-{request.File.FileName}";
                var filePath = Path.Combine(_configuration["StoredFilesPath"]!, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await request.File.CopyToAsync(stream, cancellationToken);
                }

                var document = new Document
                {
                    Name = fileName,
                    ContentType = request.File.ContentType,
                    Path = filePath,
                    DocumentStatus = DocumentStatus.New
                };

                await _documentRepository.SaveAsync(document);

                return _mapper.Map<DocumentDTO>(document);
            }
        }
    }
}
