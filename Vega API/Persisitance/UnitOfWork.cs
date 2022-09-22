using System.Threading.Tasks;
using Vega_API.Core;

namespace Vega_API.Persisitance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext context;

        public UnitOfWork(VegaDbContext context)
        {
            this.context = context;
        }
        public async Task CompleteAsync()
        {
          await  context.SaveChangesAsync();
        }
    }
}
