using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace bank_project.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(AppDb db)
        {
            Db = db;
        }

        // GET api/User
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new User(Db);
            var result = await query.GetAllAsync();
            return new OkObjectResult(result);
        }

        // GET api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new User(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);

        }

        // POST api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User body)
        {
            await Db.Connection.OpenAsync();
            body.password = BCrypt.Net.BCrypt.HashPassword(body.password);
            body.Db = Db;
            int result = await body.InsertAsync();
            Console.WriteLine("inserted id=" + result);
            if (result == 0)
            {
                return new ConflictObjectResult(0);
            }
            return new OkObjectResult(result);
        }

        // PUT api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] User body)
        {
            Console.WriteLine("update called");
            await Db.Connection.OpenAsync();
            Console.WriteLine(body);
            var query = new User(Db);
            body.password = BCrypt.Net.BCrypt.HashPassword(body.password);
    
            var result = await query.FindOneAsync(id);
            result.username=body.username;
            result.password=body.password;

            if (result is null)
                return new NotFoundResult();
            int updateTest = await result.UpdateAsync();
            if (updateTest == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkObjectResult(updateTest);
            }
        }

        // DELETE api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new User(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkObjectResult(result);
        }


        public AppDb Db { get; }
    }
}