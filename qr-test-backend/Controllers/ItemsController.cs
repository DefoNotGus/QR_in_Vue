using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

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
                    Items = JsonSerializer.Deserialize<HashSet<string>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    Items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    SaveItems();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load items: {ex.Message}");
                Items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            Console.WriteLine($"Items file path: {ItemsFile}");
        }

        private static void SaveItems()
        {
            var json = JsonSerializer.Serialize(Items.ToList(), new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(ItemsFile, json);
        }

        [HttpGet("api/items/verify")]
        public IActionResult Verify([FromQuery] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Value is required.");

            string normalized = value.Trim();

            bool exists;
            lock (Items)
            {
                exists = Items.Contains(normalized);
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

            string normalized = dto.Value.Trim();
            bool added;
            lock (Items)
            {
                if (Items.Contains(normalized))
                {
                    added = false;
                }
                else
                {
                    Items.Add(normalized);
                    SaveItems();
                    added = true;
                }
            }

            return Ok(new { added, exists = !added });
        }

        public class ItemDto
        {
            public string Value { get; set; }
        }
    }
}
