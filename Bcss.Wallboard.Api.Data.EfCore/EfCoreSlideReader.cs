using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data.EfCore.Contexts;
using Bcss.Wallboard.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bcss.Wallboard.Api.Data.EfCore
{
    public class EfCoreSlideReader : ISlideReader
    {
        private readonly SlideContext _slideContext;
        private readonly ILogger<EfCoreSlideWriter> _logger;

        public EfCoreSlideReader(SlideContext slideContext, ILogger<EfCoreSlideWriter> logger)
        {
            _slideContext = slideContext;
            _logger = logger;
        }

        public async Task<Slide> GetSlideAsync(int slideId)
        {
            _logger.LogDebug($"Retrieving slide with id {slideId}...");
            return await _slideContext.Slides.FirstOrDefaultAsync(s => s.Id == slideId);
        }
    }
}