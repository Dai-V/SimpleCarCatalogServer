using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StinkyModel;
using StinkyProjectServer.Dto;

namespace StinkyProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakesController : ControllerBase
    {
        private readonly Comp584DatabaseContext _context;

        public MakesController(Comp584DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Makes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Make>>> GetMakes()
        {
            return await _context.Makes.ToListAsync();
        }

        // GET: api/Makes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Make>> GetMake(int id)
        {
            var make = await _context.Makes.FindAsync(id);

            if (make == null)
            {
                return NotFound();
            }

            return make;
        }

        [HttpGet("GetCarAmount/{id}")]
        public async Task<ActionResult<MakeCarAmount>> GetCarAmount(int id)
        {
            MakeCarAmount make = await _context.Makes.Where(make => make.MakeId == id)
                .Select(make => new MakeCarAmount
                {
                    MakeId = make.MakeId,
                    Name = make.Name,
                    CarAmount = make.Cars.Count(),
 
                }).SingleAsync();
            return make;

        }

        // PUT: api/Makes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMake(int id, Make make)
        {
            if (id != make.MakeId)
            {
                return BadRequest();
            }

            _context.Entry(make).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MakeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Makes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Make>> PostMake(Make make)
        {
            _context.Makes.Add(make);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMake", new { id = make.MakeId }, make);
        }

        // DELETE: api/Makes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMake(int id)
        {
            var make = await _context.Makes.FindAsync(id);
            if (make == null)
            {
                return NotFound();
            }

            _context.Makes.Remove(make);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MakeExists(int id)
        {
            return _context.Makes.Any(e => e.MakeId == id);
        }
    }
}
