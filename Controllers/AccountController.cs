using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bank_project.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        public AccountController(AppDb db)
        {
            Db = db;
        }


        // GET api/account
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new AccountQuery(Db);
            var result = await query.LatestAccountAsync();
            return new OkObjectResult(result);
        }

        // GET api/account/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new AccountQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/account
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]account body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/account/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]account body)
        {
            await Db.Connection.OpenAsync();
            var query = new AccountQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.owner = body.owner;
            result.balance = body.balance;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new AccountQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/account
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new AccountQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}