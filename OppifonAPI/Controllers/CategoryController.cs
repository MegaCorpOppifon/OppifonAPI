using System.Collections.Generic;
using System.Linq;
using DAL.Factory;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.DTO;
using OppifonAPI.Helpers;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IFactory _factory;

        public CategoryController(IFactory factory) => _factory = factory;

        [HttpGet]
        public ActionResult<List<string>> GetAllCategories()
        {
            using (var unit = _factory.GetUOF())
            {
                var dbCategories = unit.Categories.GetAll();
                var categories = dbCategories.Select(dbCategory => dbCategory.Name).ToList();
                return categories;
            }
        }

        [HttpGet("{category}/expert")]
        public ActionResult<List<DTOPublicExpert>> GetExpertsInCategory(string category)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbCategory = unit.Categories.GetCategoryEagerByName(category);

                var experts = new List<DTOPublicExpert>();
                foreach (var dbExpert in dbCategory.Experts)
                {
                    var expert = new DTOPublicExpert
                    {
                        MainFields = new List<string>(),
                        ExpertTags = new List<string>(),
                        Reviews = new List<DTOReview>()
                    };

                    foreach (var review in dbExpert.Reviews)
                    {
                        var dtoReview = new DTOReview();
                        Mapper.Map(review, dtoReview);
                        expert.Reviews.Add(dtoReview);
                    }

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

                return experts;
            }
        }
    }
}
