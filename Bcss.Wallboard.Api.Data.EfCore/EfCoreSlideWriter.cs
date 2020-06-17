using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data.EfCore.Contexts;
using Bcss.Wallboard.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bcss.Wallboard.Api.Data.EfCore
{
    public class EfCoreSlideWriter : ISlideWriter
    {
        private readonly SlideContext _dbContext;
        private readonly ILogger<EfCoreSlideWriter> _logger;

        public EfCoreSlideWriter(SlideContext dbContext, ILogger<EfCoreSlideWriter> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Slide> CreateSlideAsync(string name, string content)
        {
            var slide = new Slide
            {
                Name = name,
                Content = content
            };

            var savedSlide = await _dbContext.Slides.AddAsync(slide);
            await _dbContext.SaveChangesAsync();

            return savedSlide.Entity;
        }

        public async Task<bool> DeleteSlideAsync(int slideId)
        {
            try
            {
                var slide = await _dbContext.Slides.FirstOrDefaultAsync(s => s.Id == slideId);

                if (slide == null)
                {
                    return true;
                }

                _logger.BeginScope(slide);
                _logger.LogDebug("Deleting slide...");

                _dbContext.Slides.Remove(slide);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error deleting slide");
                return false;
            }

            return true;
        }
    }
}