using Insql.Example.Contexts;
using Insql.Example.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Insql.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserDbContext dbContext;

        public ValuesController(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<UserPo> Get()
        {
            this.dbContext.GetUsers();

            return this.dbContext.GetUsers();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public UserPo Get(int id)
        {
            this.dbContext.GetUser(id);

            return this.dbContext.GetUser(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
