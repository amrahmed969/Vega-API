using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vega_API.Controllers.Resources;
using Vega_API.Models;
using Vega_API.Persisitance;

namespace Vega_API.Controllers
{
   
    public class MakesController : Controller

    {
        private VegaDbContext context;
        private readonly IMapper mapper;

        public MakesController(VegaDbContext context, IMapper mapper )
        {
            this.context = context;
            this.mapper = mapper;
        }
     
   
        [HttpGet("/api/Makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            var Makes = await context.Makes.Include(m => m.Models).ToListAsync();

            return  mapper.Map<List<Make>,List<MakeResource>>(Makes);

           
        }
    }
}
