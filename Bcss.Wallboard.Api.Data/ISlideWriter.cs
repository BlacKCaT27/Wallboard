using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data.Entities;

namespace Bcss.Wallboard.Api.Data
{
    public interface ISlideWriter
    {
        Task<Slide> CreateSlideAsync(string name, string content);

        Task<bool> DeleteSlideAsync(int slide);
    }
}