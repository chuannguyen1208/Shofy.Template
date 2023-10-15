using AutoMapper;
using MediatR;
using Shofy.UseCases.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.Documents.Queries
{
    public record GetDocumentsQuery : IRequest<IEnumerable<DocumentDTO>>
    {
        internal class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, IEnumerable<DocumentDTO>>
        {
            private readonly IDocumentRepository _documentRepository;
            private readonly IMapper _mapper;

            public GetDocumentsQueryHandler(IDocumentRepository documentRepository, IMapper mapper)
            {
                _documentRepository = documentRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<DocumentDTO>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
            {
                var docs = await _documentRepository.GetDocumentsAsync();

                var result = _mapper.Map<IEnumerable<DocumentDTO>>(docs);

                return result;
            }
        }
    }
}
