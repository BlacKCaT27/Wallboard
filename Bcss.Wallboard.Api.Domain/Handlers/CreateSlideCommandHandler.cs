using System.Threading;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data;
using Bcss.Wallboard.Api.Domain.Commands;
using Bcss.Wallboard.Api.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bcss.Wallboard.Api.Domain.Handlers
{
    public class CreateSlideCommandHandler : IRequestHandler<CreateSlideCommand, SlideResponse>
    {
        private readonly ISlideWriter _slideWriter;
        private readonly ILogger<CreateSlideCommandHandler> _logger;

        public CreateSlideCommandHandler(ISlideWriter slideWriter, ILogger<CreateSlideCommandHandler> logger)
        {
            _slideWriter = slideWriter;
            _logger = logger;
        }

        public async Task<SlideResponse> Handle(CreateSlideCommand request, CancellationToken cancellationToken)
        {
            _logger.BeginScope(request);
            _logger.LogDebug("Executing Create Slide command...");

            var slide = await _slideWriter.CreateSlideAsync(request.Name, request.Content);

            return new SlideResponse
            {
                Id = slide.Id,
                Name = slide.Name,
                Content = slide.Content
            };
        }
    }
}