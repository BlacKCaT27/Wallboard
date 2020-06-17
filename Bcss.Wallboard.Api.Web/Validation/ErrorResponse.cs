using System.Collections;
using System.Collections.Generic;

namespace Bcss.Wallboard.Api.Web.Validation
{
    public class ErrorResponse
    {
        public ICollection<Error> Errors { get; set; } = new List<Error>();
    }
}