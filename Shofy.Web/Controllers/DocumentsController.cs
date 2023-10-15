using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shofy.UseCases.Documents;
using Shofy.UseCases.Documents.Commands;
using Shofy.UseCases.Documents.Queries;

namespace Shofy.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<DocumentDTO>> GetManyAsync()
        {
            var res = await _mediator.Send(new GetDocumentsQuery());
            return res;
        }

        [HttpPost("upload")]
        public async Task<DocumentDTO> UploadAsync(IFormFile formFile)
        {
            var res = await _mediator.Send(new UploadDocumentCommand(formFile));
            return res;
        }
    }
}
