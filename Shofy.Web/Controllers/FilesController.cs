using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shofy.UseCases.Files;
using Shofy.UseCases.Files.Commands;

namespace Shofy.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<FileDTO> UploadAsync(IFormFile formFile)
        {
            var res = await _mediator.Send(new UploadFileCommand(formFile));
            return res;
        }
    }
}
