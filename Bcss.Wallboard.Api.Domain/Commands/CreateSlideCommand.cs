using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Bcss.Wallboard.Api.Domain.Constants;
using Bcss.Wallboard.Api.Domain.Responses;
using MediatR;

namespace Bcss.Wallboard.Api.Domain.Commands
{
    public class CreateSlideCommand : IRequest<SlideResponse>
    {
        [Required]
        [StringLength(ValidationConstants.MaxSlideNameLength, MinimumLength = 1, ErrorMessage = "Name length is outside of the allowed range")]
        public string Name { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxSlideContentLength, MinimumLength = 1, ErrorMessage = "Content length is outside of the allowed range")]
        public string Content { get; set; }
    }
}