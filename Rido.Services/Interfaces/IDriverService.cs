using System.Threading.Tasks;
using Rido.Model.Requests;

namespace Rido.Services.Interfaces
{
    public interface IDriverService
    {
        Task<string> RegisterDriverAsync(RegisterDriverDto dto);
    }
}
