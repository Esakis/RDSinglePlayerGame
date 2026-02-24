using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClickTrackerAPI.Data;
using ClickTrackerAPI.Models;

namespace ClickTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClicksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClicksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetClickCount()
        {
            var count = await _context.ClickEvents.CountAsync();
            return Ok(count);
        }

        [HttpPost]
        public async Task<ActionResult<ClickEvent>> RecordClick()
        {
            var clickEvent = new ClickEvent
            {
                ClickedAt = DateTime.UtcNow
            };

            _context.ClickEvents.Add(clickEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClickCount), new { id = clickEvent.Id }, clickEvent);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClickEvent>>> GetAllClicks()
        {
            return await _context.ClickEvents.OrderByDescending(c => c.ClickedAt).ToListAsync();
        }
    }
}
