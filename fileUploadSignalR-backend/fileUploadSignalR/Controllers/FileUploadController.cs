using Application.Command;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace webAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FileUploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var command = new ProcessFileCommand(file);
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet("processedRows")]
        public async Task<ActionResult<IEnumerable<string>>> GetProcessedRows()
        {
            var query = new GetProcessedRowsQuery();
            var rows = await _mediator.Send(query);
            return Ok(rows);
        }
    }
}
