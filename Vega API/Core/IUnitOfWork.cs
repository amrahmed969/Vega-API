 using System.Threading.Tasks;

namespace Vega_API.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
