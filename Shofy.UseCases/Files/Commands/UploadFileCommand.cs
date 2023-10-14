using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.Files.Commands
{
    public record UploadFileCommand(IFormFile File) : IRequest<FileDTO>
    {
        internal class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileDTO>
        {
            private readonly IConfiguration _configuration;
            public UploadFileCommandHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<FileDTO> Handle(UploadFileCommand request, CancellationToken cancellationToken)
            {
                var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}-Shofy-{request.File.FileName}";
                var filePath = Path.Combine(_configuration["StoredFilesPath"]!, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await request.File.CopyToAsync(stream, cancellationToken);
                }

                return new FileDTO
                {
                    Name = fileName,
                    ContentType = request.File.ContentType,
                    Path = filePath
                };
            }
        }
    }
}
