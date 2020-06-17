using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bcss.Wallboard.Api.Web.Validation
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // If the model state is invalid, collect the errors into an ErrorResponse and short-circuit the request.
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(pair => pair.Key, pair => pair.Value.Errors.Select(x => x.ErrorMessage))
                    .ToArray();

                var errorResponse = new ErrorResponse();

                foreach (var (key, value) in errorsInModelState)
                {
                    foreach (var subError in value)
                    {
                        errorResponse.Errors.Add(new Error
                        {
                            Field = key,
                            Message = subError
                        });
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}