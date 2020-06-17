using System.Threading;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data;
using Bcss.Wallboard.Api.Domain.Commands;
using Bcss.Wallboard.Api.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bcss.Wallboard.Api.Domain.Handlers
{
    public class DeleteSlideCommandHandler : IRequestHandler<DeleteSlideCommand, SlideResponse>
    {
        private readonly ISlideWriter _slideWriter;
        private readonly ILogger<DeleteSlideCommandHandler> _logger;

        public DeleteSlideCommandHandler(ISlideWriter slideWriter, ILogger<DeleteSlideCommandHandler> logger)
        {
            _slideWriter = slideWriter;
            _logger = logger;
        }

        public async Task<SlideResponse> Handle(DeleteSlideCommand request, CancellationToken cancellationToken)
        {
            _logger.BeginScope(request);
            _logger.LogDebug("Deleting slide...");

            var result = await _slideWriter.DeleteSlideAsync(request.Id);

            return result ? new SlideResponse{ Id = request.Id } : null;
        }
    }
}