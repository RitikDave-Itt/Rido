using System.Threading.Tasks;
using Rido.Common.Models.Requests;

namespace Rido.Services.Interfaces
{
    public interface IDriverService
    {
        Task<string> RegisterDriverAsync(RegisterDriverDto dto);
    }
}
