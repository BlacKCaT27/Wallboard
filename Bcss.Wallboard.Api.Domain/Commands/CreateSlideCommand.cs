using Bcss.Wallboard.Api.Domain.Responses;
using MediatR;

namespace Bcss.Wallboard.Api.Domain.Commands
{
    public class CreateSlideCommand : IRequest<SlideResponse>
    {
        public string Name { get; set; }

        public string Content { get; set; }
    }
}