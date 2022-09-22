using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Vega_API.Core;
using Vega_API.Models;

namespace Vega_API.Persisitance
{
    public class VehicleRepository : IVehicleRepository
    {
        public VehicleRepository(VegaDbContext context)
        {
            this.context = context;
        }

        public VegaDbContext context { get; }

        public async Task<Vehicle> GetVehicle(int id,bool includRelated = true)
        {
            if (!includRelated )
                return await context.Vehicles.FindAsync(id); 

             return  await context.Vehicles
                .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
                .Include(v => v.Model)
                .ThenInclude(m => m.Make)
                .SingleOrDefaultAsync(v => v.Id == id);          
        }


        public void Add(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);  
        }

        public void Remove(Vehicle vehicle )
        {
            context.Remove(vehicle);
        }
    }
}
