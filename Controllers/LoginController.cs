using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using bank_project;

namespace bank_project.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public LoginController(AppDb db)
        {
            Db = db;
        }


        // POST api/login
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User body)
        {
            Console.WriteLine(body.username);
            Console.WriteLine(body.password);
            await Db.Connection.OpenAsync();
            var query = new Login(Db);
            var result = await query.GetPassword(body.username);
   
            if (result is null || ! BCrypt.Net.BCrypt.Verify(body.password, result))
            {
                // authentication failed
                return new OkObjectResult(false);
            }
            else
            {
                // authentication successful
                return new OkObjectResult(true);
                Singleton singObject=Singleton.Instance;
                singObject.Username=body.username;
                singObject.Password=body.password;
            }
            
        }

    

        public AppDb Db { get; }
    }
}