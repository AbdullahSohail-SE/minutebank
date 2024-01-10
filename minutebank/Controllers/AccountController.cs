using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minutebank.Base;
using minutebank.Mapper;
using minutebank.Models;
using System.Data.SqlClient;
using System.Security.Principal;

namespace minutebank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IDbConnectionClass _dbConnectionClass;

        public AccountController(IDbConnectionClass dbConnectionClass)
        {
            _dbConnectionClass = dbConnectionClass;
        }

        public static string GenerateAccountNumber()
        {
            // Create a random number generator
            Random random = new Random();

            // Generate a 24-digit account number
            string accountNumber = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                accountNumber += random.Next(0, 10).ToString();
            }

            return accountNumber;
        }



        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            try
            {
                account.account_number = GenerateAccountNumber();
                account.balance = 0;

                var parameters = new Dictionary<string, object>
                    {
                        { "@account_number", account.account_number },
                        { "@balance",account.balance },
                        { "@type", account.type },
                        { "@status", account.status },
                        { "@user_id", account.user_id },
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [Account] VALUES ({paramCollection})";

                var accountId = _dbConnectionClass.AddEntity<Account>(insertQuery, parameters);

                var newAccount = _dbConnectionClass.GetEntities<Account>($"SELECT * FROM [Account] WHERE id = {accountId}", DBMapper.accountMapper);

                return Created($"/account/get/{accountId}", newAccount);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetAccount(int user_id)
        {
            try
            {
                var query = $"SELECT * FROM [Account] WHERE user_id = {user_id}";

                var accounts = _dbConnectionClass.GetEntities<Account>(query, DBMapper.accountMapper);

                return Ok(accounts);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }




    }
}
