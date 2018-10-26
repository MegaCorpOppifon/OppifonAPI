using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            IEnumerable<Expert> dbExperts;

            using (var unit = _factory.GetUOF())
            {
                dbExperts = unit.Experts.GetAllEager();
            }

            foreach (var dbExpert in dbExperts)
            {
                var dtoExpert = new DTOExpert
                {
                    Reviews = new List<DTOReview>(),
                    MainFields = new List<string>(),
                    ExpertTags = new List<string>(),
                };
                Mapper.Map(dbExpert, dtoExpert);
                dtoExpert.ExpertCategory = dbExpert.ExpertCategory.Name;

                foreach (var dbExpertReview in dbExpert.Reviews)
                {
                    var review = new DTOReview();
                    Mapper.Map(dbExpertReview, review);
                    dtoExpert.Reviews.Add(review);
                }

                foreach (var dbExpertMainField in dbExpert.MainFields)
                {
                    dtoExpert.MainFields.Add(dbExpertMainField.Tag.Name);
                }

                foreach (var dbExpertTag in dbExpert.ExpertTags)
                {
                    dtoExpert.ExpertTags.Add(dbExpertTag.Tag.Name);
                }
                dtoExperts.Add(dtoExpert);
            }

            return Ok(dtoExperts);
        }

        // GET: api/Expert/5
        [HttpGet("{id}")]
        public IActionResult GetExpert(Guid id)
        {
            var dtoExpert = new DTOExpert
            {
                Reviews = new List<DTOReview>(),
                MainFields = new List<string>(),
                ExpertTags = new List<string>(),
            };
            Expert dbExpert;

            using (var unit = _factory.GetUOF())
            {
                dbExpert = unit.Experts.GetEager(id);
            }

            if (dbExpert == null)
                return BadRequest(new
                    {message = $"Expert with id '{id}' did not exist."});

            Mapper.Map(dbExpert, dtoExpert);

            dtoExpert.ExpertCategory = dbExpert.ExpertCategory.Name;

            foreach (var dbExpertReview in dbExpert.Reviews)
            {
                var review = new DTOReview();
                Mapper.Map(dbExpertReview, review);
                dtoExpert.Reviews.Add(review);
            }

            foreach (var dbExpertMainField in dbExpert.MainFields)
            {
                dtoExpert.MainFields.Add(dbExpertMainField.Tag.Name);
            }

            foreach (var dbExpertTag in dbExpert.ExpertTags)
            {
                dtoExpert.ExpertTags.Add(dbExpertTag.Tag.Name);
            }

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
        public void UpdateExpert(Guid id, [FromBody] DTOExpert dtoExpert)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbExpert = unit.Experts.Get(id);
                dbExpert.Description = dtoExpert.Description ?? dbExpert.Description;
                dbExpert.City = dtoExpert.City ?? dbExpert.City;
                dbExpert.Email = dtoExpert.Email ?? dbExpert.Email;
                dbExpert.FirstName = dtoExpert.FirstName ?? dbExpert.FirstName;
                dbExpert.LastName = dtoExpert.LastName ?? dbExpert.LastName;
                dbExpert.Gender = dtoExpert.Gender ?? dbExpert.Gender;
                dbExpert.PhoneNumber = dtoExpert.PhoneNumber ?? dbExpert.PhoneNumber;
                dbExpert.Birthday = dtoExpert.Birthday == default(DateTime)
                    ? dtoExpert.Birthday
                    : dbExpert.Birthday;

                if (dtoExpert.InterestTags != null)
                {
                    dbExpert.InterestTags = new Collection<UserTag>();
                    foreach (var interestTag in dtoExpert.InterestTags)
                    {
                        var tag = unit.Tags.GetTagByName(interestTag) ??
                                  new Tag { Name = interestTag };

                        var userTag = new UserTag
                        {
                            Tag = tag,
                            User = dbExpert
                        };

                        dbExpert.InterestTags.Add(userTag);
                    }
                }

                if (dtoExpert.ExpertTags != null)
                {
                    dbExpert.ExpertTags = new Collection<ExpertTag>();
                    foreach (var expertTag in dtoExpert.ExpertTags)
                    {
                        var tag = unit.Tags.GetTagByName(expertTag) ??
                                  new Tag { Name = expertTag };

                        var newExpertTag = new ExpertTag
                        {
                            Tag = tag,
                            Expert = dbExpert
                        };

                        dbExpert.ExpertTags.Add(newExpertTag);
                    }
                }

                if (dtoExpert.MainFields != null)
                {
                    dbExpert.MainFields = new Collection<MainFieldTag>();
                    foreach (var mainFieldTag in dtoExpert.MainFields)
                    {
                        var tag = unit.Tags.GetTagByName(mainFieldTag) ??
                                  new Tag { Name = mainFieldTag };

                        var newMainFieldTag = new MainFieldTag
                        {
                            Tag = tag,
                            Expert = dbExpert
                        };

                        dbExpert.MainFields.Add(newMainFieldTag);
                    }
                }

                if (dtoExpert.ExpertCategory != null)
                {
                    var category = unit.Categories.GetCategoryByName(dtoExpert.ExpertCategory);
                    dbExpert.ExpertCategory = category;
                }

                unit.Complete();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("{expertId}/review")]
        public IActionResult AddReview([FromBody] DTOReview dtoReview,Guid expertId)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbExpert = unit.Experts.GetEager(expertId);
                if (dbExpert == null)
                    return BadRequest(new {message = $"Expert with id '{expertId}' did not exist"});

                var review = new Review();
                Mapper.Map(dtoReview, review);

                if (dtoReview.Anonymity)
                    review.Name = "";
                
                dbExpert.Reviews.Add(review);
                unit.Complete();

                return Ok();
            }
        }

        [HttpGet("{expertId}/review")]
        public IActionResult GetReviews(Guid expertId)
        {
            Expert dbExpert;
            using (var unit = _factory.GetUOF())
            {
                dbExpert = unit.Experts.GetEager(expertId);
                if (dbExpert == null)
                    return BadRequest(new {message = $"Expert with id '{expertId}' did not exist"});
            }

            var reviews = new List<DTOReview>();

            foreach (var review in dbExpert.Reviews)
            {
                var dtoReview = new DTOReview();
                Mapper.Map(review, dtoReview);
                reviews.Add(dtoReview);
            }
            
            return Ok(reviews);
            
        }

        [HttpGet("{expertId}/review/{reviewId}")]
        public IActionResult GetReviews(Guid expertId, Guid reviewId)
        {
            Review dbReview;
            using (var unit = _factory.GetUOF())
            {
                var dbExpert = unit.Experts.GetEager(expertId);
                if (dbExpert == null)
                    return BadRequest(new { message = $"Expert with id '{expertId}' did not exist" });

                dbReview = dbExpert.Reviews.FirstOrDefault(x => x.Id == reviewId);
                if(dbReview == null)
                    return BadRequest(new { message = $"Review with id '{reviewId}' did not exist" });
            }

            var dtoReview = new DTOReview();
            Mapper.Map(dbReview, dtoReview);
            
            return Ok(dtoReview);
        }
    }
}