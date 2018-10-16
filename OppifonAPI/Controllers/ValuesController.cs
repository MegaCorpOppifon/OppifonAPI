using System.Collections.Generic;
using DAL.Factory;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IFactory _factory;
        public ValuesController(IFactory factory)
        {
            _factory = factory;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("test")]
        public string Test(int id)
        {
            return "test";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            using (var work = _factory.GetUOF())
            {
                var tag = new Tag
                {
                    Name = "Healt"
                };
                work.Tags.Add(tag);
                work.Complete();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
