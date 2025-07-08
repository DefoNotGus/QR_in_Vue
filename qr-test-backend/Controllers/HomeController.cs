using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("api/hardcoded")]
        public JsonResult GetHardcodedData()
        {
            var data = GetMockData();
            return Json(data);
        }

        [HttpGet("api/hardcoded/verify")]
        public IActionResult Verify([FromQuery] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Value is required.");

            var data = GetMockData();

            var match = data.FirstOrDefault(item =>
                item.Name.Equals(value, System.StringComparison.OrdinalIgnoreCase) ||
                item.Id.ToString() == value
            );

            if (match != null)
                return Ok(match);

            return NotFound("Item not found.");
        }

        private List<Item> GetMockData()
        {
            return new List<Item>
            {
                new Item { Id = 1, Name = "Item A", Value = 10 },
                new Item { Id = 2, Name = "Item B", Value = 20 },
                new Item { Id = 3, Name = "Item C", Value = 30 }
            };
        }

        private class Item
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }
}
