using System.Collections.Generic;
using DAL.Factory;
using Microsoft.AspNetCore.Mvc;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertController : ControllerBase
    {
        private readonly IFactory _factory;

        public ExpertController(IFactory factory)
        {
            _factory = factory;
        }

        // GET: api/Expert
        [HttpGet]
        public IActionResult GetAllExperts()
        {
            using (var unit = _factory.GetUOF())
            {
                var experts = unit.Experts.GetAll();
                return Ok(experts);
            }
            
        }

        // GET: api/Expert/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Expert
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
