using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly List<string> Items = new()
        {
            "demo-1",
            "demo-2"
        };

        [HttpGet("api/items/verify")]
        public IActionResult Verify([FromQuery] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Value is required.");

            if (Items.Contains(value))
                return Ok(new { exists = true });

            return NotFound(new { exists = false });
        }

        [HttpPost("api/items")]
        public IActionResult Add([FromBody] ItemDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Value))
                return BadRequest("Value is required.");

            Items.Add(dto.Value);
            return Ok(new { added = true });
        }

        public class ItemDto
        {
            public string Value { get; set; }
        }
    }
}
