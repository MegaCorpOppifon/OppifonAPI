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
        public ActionResult<List<string>> GetAllTags()
        {
            using (var unit = _factory.GetUOF())
            {
                var dbTags = unit.Tags.GetAll();

                var tags = dbTags.Select(dbTag => dbTag.Name).ToList();

                return tags;
            }
        }

        [HttpGet("{tag}/expert")]
        public ActionResult<List<DTOPublicExpert>> GetAllExpertsWithTag(string tag)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbExperts = unit.Experts.GetExpertsWithTagName(tag);

                var dtoExperts = new List<DTOPublicExpert>();
                foreach (var dbExpert in dbExperts)
                {
                    var dtoExpert = new DTOPublicExpert
                    {
                        MainFields = new List<string>(),
                        ExpertTags = new List<string>(),
                        Reviews = new List<DTOReview>()
                    };
                    Mapper.Map(dbExpert, dtoExpert);
                    dtoExpert.ExpertCategory = dbExpert.ExpertCategory.Name;

                    foreach (var dbExpertReview in dbExpert.Reviews)
                    {
                        var review = new DTOReview();
                        Mapper.Map(dbExpertReview, review);
                        dtoExpert.Reviews.Add(review);
                    }
                    
                    foreach (var mainField in dbExpert.MainFields)
                    {
                        dtoExpert.MainFields.Add(mainField.Tag.Name);
                    }

                    foreach (var expertTag in dbExpert.ExpertTags)
                    {
                        dtoExpert.ExpertTags.Add(expertTag.Tag.Name);
                    }

                    dtoExperts.Add(dtoExpert);
                }

                return dtoExperts;
            }
        }
    }
}
