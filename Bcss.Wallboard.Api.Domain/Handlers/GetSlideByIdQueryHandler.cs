using System.Threading;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data;
using Bcss.Wallboard.Api.Domain.Queries;
using Bcss.Wallboard.Api.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bcss.Wallboard.Api.Domain.Handlers
{
    public class GetSlideByIdQueryHandler : IRequestHandler<GetSlideByIdQuery, SlideResponse>
    {
        private readonly ISlideReader _slideReader;
        private readonly ILogger<GetSlideByIdQueryHandler> _logger;

        public GetSlideByIdQueryHandler(ISlideReader slideReader, ILogger<GetSlideByIdQueryHandler> logger)
        {
            _slideReader = slideReader;
            _logger = logger;
        }

        public async Task<SlideResponse> Handle(GetSlideByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.BeginScope(request);

            var slide = await _slideReader.GetSlideAsync(request.Id);

            return new SlideResponse
            {
                Id = slide.Id,
                Name = slide.Name,
                Content = slide.Content
            };
        }
    }
}