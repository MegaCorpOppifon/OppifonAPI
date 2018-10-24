using System;
using System.Collections.Generic;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.DTO;
using OppifonAPI.Helpers;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ExpertController : ControllerBase
    {
        private readonly IFactory _factory;

        public ExpertController(IFactory factory) => _factory = factory;

        // GET: api/Expert
        [HttpGet]
        public IActionResult GetAllExperts()
        {
            var dtoExperts = new List<DTOExpert>();
            IEnumerable<Expert> experts;

            using (var unit = _factory.GetUOF())
            {
                experts = unit.Experts.GetAll();
            }

            foreach (var expert in experts)
            {
                var dtoExpert = new DTOExpert();
                Mapper.Map(expert, dtoExpert);
                dtoExperts.Add(dtoExpert);
            }

            return Ok(dtoExperts);
        }

        // GET: api/Expert/5
        [HttpGet("{id}")]
        public IActionResult GetExpert(Guid id)
        {
            var dtoExpert = new DTOExpert();
            Expert expert;

            using (var unit = _factory.GetUOF())
            {
                expert = unit.Experts.Get(id);
            }

            if (expert == null)
                return BadRequest(new
                    {message = $"Expert with id '{id}' did not exist."});

            Mapper.Map(expert, dtoExpert);
            return Ok(dtoExpert);
        }

        // POST: api/Expert
        [HttpPost]
        public IActionResult UpgradeToExpert([FromBody] DTOExpert dtoExpert)
        {
            using (var unit = _factory.GetUOF())
            {
                var expert = new Expert();
                var dbUser = unit.Users.Get(dtoExpert.Id);

                if (dbUser.IsExpert)
                    return BadRequest(new {message = "User is already an expert"});

                Mapper.Map(dbUser, expert);

                expert.ExpertTags = new List<ExpertTag>();
                expert.MainFields = new List<MainFieldTag>();
                expert.IsExpert = true;
                var category = unit.Categories.GetCategoryEagerByName(dtoExpert.ExpertCategory);

                foreach (var expertTag in dtoExpert.ExpertTags)
                {
                    var tag = unit.Tags.GetTagByName(expertTag) ?? new Tag { Name = expertTag };

                    category.Tags.Add(tag);

                    var newExpertTag = new ExpertTag
                    {
                        Expert = expert,
                        Tag = tag
                    };

                    expert.ExpertTags.Add(newExpertTag);
                }

                unit.Complete();

                foreach (var expertTag in dtoExpert.MainFields)
                {
                    var tag = unit.Tags.GetTagByName(expertTag) ?? new Tag { Name = expertTag };

                    category.Tags.Add(tag);

                    var newMainField = new MainFieldTag()
                    {
                        Expert = expert,
                        Tag = tag
                    };

                    expert.MainFields.Add(newMainField);
                }

                expert.ExpertCategory = category;
                expert.Id = dbUser.Id;

                unit.Users.RemoveById(dbUser.Id);
                unit.Experts.Add(expert);

                unit.Complete();
            }

            return Ok();
        }

        // PUT: api/Expert/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}