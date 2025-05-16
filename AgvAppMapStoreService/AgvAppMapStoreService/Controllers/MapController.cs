using AgvAppMapStoreService.Core;
using AgvAppMapStoreService.Data;
using AgvAppMapStoreService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgvAppMapStoreService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MapsController : ControllerBase
{
    private readonly MapDbContext _context;

    public MapsController(MapDbContext context)
    {
        _context = context;
    }

    // POST: api/maps
    [HttpPost]
    public async Task<ActionResult<MapReadModel>> CreateMap([FromBody] MapCreateModel MapCreateModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var map = new Map
        {
            Id = Guid.NewGuid(),
            Name = MapCreateModel.Name,
            SvgContent = MapCreateModel.SvgContent,
            CreatedAt = DateTime.UtcNow
        };

        _context.Maps.Add(map);
        await _context.SaveChangesAsync();

        var MapReadModel = new MapReadModel
        {
            Id = map.Id,
            Name = map.Name,
            SvgContent = map.SvgContent,
            CreatedAt = map.CreatedAt
        };

        return CreatedAtAction(nameof(GetMap), new { id = map.Id }, MapReadModel);
    }

    // GET: api/maps
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MapReadModel>>> GetMaps()
    {
        var maps = await _context.Maps
            .Select(m => new MapReadModel
            {
                Id = m.Id,
                Name = m.Name,
                SvgContent = m.SvgContent,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync();

        return Ok(maps);
    }

    // GET: api/maps/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MapReadModel>> GetMap(Guid id)
    {
        var map = await _context.Maps.FindAsync(id);

        if (map == null)
        {
            return NotFound();
        }

        var MapReadModel = new MapReadModel
        {
            Id = map.Id,
            Name = map.Name,
            SvgContent = map.SvgContent,
            CreatedAt = map.CreatedAt
        };

        return Ok(MapReadModel);
    }

    // DELETE: api/maps/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMap(Guid id)
    {
        var map = await _context.Maps.FindAsync(id);
        if (map == null)
        {
            return NotFound();
        }

        _context.Maps.Remove(map);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}