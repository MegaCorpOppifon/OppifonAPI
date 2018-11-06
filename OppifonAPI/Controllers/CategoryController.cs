using System.Collections.Generic;
using System.Linq;
using DAL.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.DTO;
using OppifonAPI.Helpers;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IFactory _factory;

        public CategoryController(IFactory factory) => _factory = factory;

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            using (var unit = _factory.GetUOF())
            {
                var dbCategories = unit.Categories.GetAll();
                var categories = dbCategories.Select(dbCategory => dbCategory.Name).ToList();
                return Ok(categories);
            }
        }

        [HttpGet("{category}/expert")]
        public IActionResult GetExpertsInCategory(string category)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbCategory = unit.Categories.GetCategoryEagerByName(category);

                var experts = new List<DTOExpert>();
                foreach (var dbExpert in dbCategory.Experts)
                {
                    var expert = new DTOExpert
                    {
                        MainFields = new List<string>(),
                        ExpertTags = new List<string>()
                    };
                    Mapper.Map(dbExpert, expert);
                    expert.ExpertCategory = dbExpert.ExpertCategory.Name;

                    foreach (var mainField in dbExpert.MainFields)
                    {
                        expert.MainFields.Add(mainField.Tag.Name);
                    }

                    foreach (var expertTag in dbExpert.ExpertTags)
                    {
                        expert.ExpertTags.Add(expertTag.Tag.Name);
                    }

                    experts.Add(expert);
                }

                return Ok(experts);
            }
        }
    }
}
