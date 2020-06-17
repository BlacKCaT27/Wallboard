using System.Threading.Tasks;
using Bcss.Wallboard.Api.Domain.Responses;
using MediatR;

namespace Bcss.Wallboard.Api.Domain.Commands
{
    public class DeleteSlideCommand : IRequest<SlideResponse>
    {
        public int Id { get; }

        public DeleteSlideCommand(int id)
        {
            Id = id;
        }
    }
}