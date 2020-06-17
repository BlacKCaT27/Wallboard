using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data.Entities;

namespace Bcss.Wallboard.Api.Data
{
    public interface ISlideReader
    {
        Task<Slide> GetSlideAsync(int slideId);
    }
}