using System.Net;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Domain.Commands;
using Bcss.Wallboard.Api.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bcss.Wallboard.Api.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SlidesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SlidesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{slideId}")]
        public async Task<IActionResult> GetSlide(int slideId)
        {
            var query = new GetSlideByIdQuery(slideId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlide([FromBody] CreateSlideCommand createSlideCommand)
        {
            var result = await _mediator.Send(createSlideCommand);
            return CreatedAtAction(nameof(GetSlide), new {slideId = result.Id}, result);
        }

        [HttpDelete("{slideId}")]
        public async Task<IActionResult> DeleteSlide(int slideId)
        {
            var command = new DeleteSlideCommand(slideId);
            var result = await _mediator.Send(command);

            // Here, we want DeleteSlide to be idempotent and continue to report success when deleting items
            // that no longer exist, since from the callers perspective, the new state they requested (for the
            // item to not exist any longer) is still satisfied. Therefore we only do not return Ok if we detect
            // an error in processing, which is represented by result being null.
            return result != null ? Ok() : new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}