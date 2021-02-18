using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

            foreach (var m in mm)
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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var m = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();

            if (m == null) return NotFound();
            m.Name = celestialObject.Name;
            m.OrbitalPeriod = celestialObject.OrbitalPeriod;
            m.OrbitedObjectId = celestialObject.OrbitedObjectId;

            //m = celestialObject;
            _context.Update(m);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var m = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (m == null) return NotFound();
            m.Name = name;
            _context.Update(m);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var m = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (m == null) return NotFound();
            _context.Remove(m);
            _context.SaveChanges();

            return NoContent();
        }


    }
}
