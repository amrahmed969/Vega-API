using System.Collections.Generic;
using System.Threading.Tasks;
using Vega_API.Core.Models;
using Vega_API.Models;

namespace Vega_API.Core
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicle(int id, bool includRelated = true);
        void Add(Vehicle vehicle);
        void Remove(Vehicle vehicle);

        Task<IEnumerable<Vehicle>> GetVehicles(VehicleQuery filter);
    }

}
