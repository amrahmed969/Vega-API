using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vega_API.Controllers.Resources;
using Vega_API.Core;
using Vega_API.Core.Models;
using Vega_API.Extensions;
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

        public async Task<IEnumerable<Vehicle>> GetVehicles(VehicleQuery queryObj)
        {
            var query = context.Vehicles
                 .Include(v => v.Model)
                 .ThenInclude(v => v.Make)
                 .Include(v => v.Features)
                 .ThenInclude(vf => vf.Feature)
                 .AsQueryable();
           
            //filtering
            if (queryObj.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);

            if (queryObj.ModelId.HasValue)
                query = query.Where(v => v.ModelId == queryObj.ModelId.Value);

            //sorting
            var columnMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"]= v=>v.Model.Make.Name,
                ["model"]= v=>v.Model.Name,
                ["contactName"]= v=>v.ContactName,
              
            };
 
            query = query.ApplayOrdering(queryObj, columnMap);

            //if (queryObj.SortBy == "make")
            //    query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);

            //if (queryObj.SortBy == "model")
            //    query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Model.Name) : query.OrderByDescending(v => v.Model.Name);

            //if (queryObj.SortBy == "contactName")
            //    query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.ContactName) : query.OrderByDescending(v => v.ContactName);

            //if (queryObj.SortBy == "id")
            //    query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Id) : query.OrderByDescending(v => v.Id);

            // pagination

            query = query.ApplayPaging(queryObj);
            return await query.ToListAsync();
        }

        
    }
}
