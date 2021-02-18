using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]

        public IActionResult GetById(int id)
        {
            var m = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();

            if (m == null) return NotFound();
            m.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
            return Ok(m);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {

            var mm = _context.CelestialObjects.Where(x => x.Name.Contains(name));

            if (mm.Count() == 0) return NotFound();

            foreach(var m in mm)
            {
                m.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == m.Id).ToList();

            }
            return Ok(mm);
        }

        [HttpGet(Name = "GetAll")]

        public IActionResult GetAll()
        {
            return GetByName("");
        }

    }
}
