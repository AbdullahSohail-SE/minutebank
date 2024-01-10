using Microsoft.AspNetCore.Mvc;
using minutebank.Base;
using minutebank.Mapper;
using minutebank.Models;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace minutebank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDbConnectionClass _dbConnectionClass;

        public UserController(IDbConnectionClass dbConnectionClass) {
            _dbConnectionClass = dbConnectionClass;
        }

        

        // GET: api/<ValuesController>
        [HttpPost("login")]
        public IActionResult SigninUser([FromBody] User user)
        {
            try
            {
                var query = $"SELECT * FROM [User] WHERE email = '{user.email}' AND [password] = '{user.password}'";

                var currentUser = _dbConnectionClass.GetEntity<User>(query, DBMapper.userMapper);
    
                if(currentUser != null)
                    return Ok(currentUser);

                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong"});
                throw;
            }
           
        }
        [HttpPost]
        public IActionResult SignupUser([FromBody]User user)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                    {
                        { "@name", user?.name },
                        { "@email",user?.email },
                        { "@password", user?.password },
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [USER] VALUES ({ paramCollection })";

                var userId = _dbConnectionClass.AddEntity<User>(insertQuery,parameters);

                var newUser = _dbConnectionClass.GetEntity<User>($"SELECT * FROM [USER] WHERE id = {userId}", DBMapper.userMapper);

                return Created($"/user/get/{userId}" , newUser);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }

        [HttpGet("stats")]
        public IActionResult GetActivityStats(int id)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                    {
                       { "@user_id", id },
                    };

                var accountStats = _dbConnectionClass.ExecuteStoredProcedure<AccountStats>("AccountStats", parameters, DBMapper.accountStatsMapper);

                return Ok(accountStats);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }

        [HttpGet("summary")]
        public IActionResult GetSummary(int id)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                    {
                       { "@user_id", id },
                    };

                var summary = _dbConnectionClass.ExecuteStoredProcedure<Summary>("Summary", parameters, DBMapper.summaryMapper).FirstOrDefault();

                return Ok(summary);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }

    }
}
