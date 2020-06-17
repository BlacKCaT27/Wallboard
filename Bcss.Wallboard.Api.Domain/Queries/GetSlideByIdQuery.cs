using Bcss.Wallboard.Api.Domain.Responses;
using MediatR;

namespace Bcss.Wallboard.Api.Domain.Queries
{
    public class GetSlideByIdQuery : IRequest<SlideResponse>
    {
        public int Id { get; }

        public GetSlideByIdQuery(int id)
        {
            Id = id;
        }
    }
}