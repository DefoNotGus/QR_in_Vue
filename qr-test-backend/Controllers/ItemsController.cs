using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly string DataDir = Path.Combine(AppContext.BaseDirectory, "data");
        private static readonly string ItemsFile = Path.Combine(DataDir, "items.json");
        private static readonly HashSet<string> Items;

        static ItemsController()
        {
            try
            {
                Directory.CreateDirectory(DataDir);
                if (System.IO.File.Exists(ItemsFile))
                {
                    var json = System.IO.File.ReadAllText(ItemsFile);
                    Items = JsonSerializer.Deserialize<HashSet<string>>(json) ?? new HashSet<string>();
                }
                else
                {
                    Items = new HashSet<string>();
                    SaveItems();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load items: {ex.Message}");
                Items = new HashSet<string>();
            }

            Console.WriteLine($"Items file path: {ItemsFile}");
        }

        private static void SaveItems()
        {
            var json = JsonSerializer.Serialize(Items, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(ItemsFile, json);
        }

        [HttpGet("api/items/verify")]
        public IActionResult Verify([FromQuery] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Value is required.");

            bool exists;
            lock (Items)
            {
                exists = Items.Contains(value);
            }

            if (exists)
                return Ok(new { exists = true });

            return NotFound(new { exists = false });
        }

        [HttpPost("api/items")]
        public IActionResult Add([FromBody] ItemDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Value))
                return BadRequest("Value is required.");

            bool added;
            lock (Items)
            {
                if (Items.Contains(dto.Value))
                {
                    added = false;
                }
                else
                {
                    Items.Add(dto.Value);
                    SaveItems();
                    added = true;
                }
            }

            return Ok(new { added });
        }

        public class ItemDto
        {
            public string Value { get; set; }
        }
    }
}
