using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IFactory _factory;

        public CategoryController(IFactory factory) => _factory = factory;

        // GET: api/Category
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            using (var unit = _factory.GetUOF())
            {
                var dbCategories = unit.Categories.GetAll();
                var categories = dbCategories.Select(dBcategory => dBcategory.Name).ToList();
                return Ok(categories);
            }
        }
    }
}
