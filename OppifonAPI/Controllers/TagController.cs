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
    public class TagController : ControllerBase
    {
        private readonly IFactory _factory;

        public TagController(IFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            using (var unit = _factory.GetUOF())
            {
                var dbTags = unit.Tags.GetAll();

                var tags = dbTags.Select(dbTag => dbTag.Name).ToList();

                return Ok(tags);
            }
        }

        [HttpGet("{tag}/expert")]
        public IActionResult GetAllExpertsWithTag(string tag)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbExperts = unit.Experts.GetExpertsWithTagName(tag);

                var experts = new List<DTOExpert>();
                foreach (var dbExpert in dbExperts)
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
