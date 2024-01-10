using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minutebank.Base;
using minutebank.Mapper;
using minutebank.Models;
using System.Data.SqlClient;

namespace minutebank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        private readonly IDbConnectionClass _dbConnectionClass;

        public RecipientController(IDbConnectionClass dbConnectionClass)
        {
            _dbConnectionClass = dbConnectionClass;
        }



        [HttpPost]
        public IActionResult CreateRecipient([FromBody] Recipient recipient)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                    {
                        { "@name", recipient.name },
                        { "@bank", recipient.bank },
                        { "@swift_code", recipient.swift_code },
                        { "@account_number", recipient.account_number },
                        { "@user_id", recipient.user_id },
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [Recipient] VALUES ({paramCollection})";

                var recipientId = _dbConnectionClass.AddEntity<Recipient>(insertQuery, parameters);

                var newRecipient = _dbConnectionClass.GetEntities<Recipient>($"SELECT * FROM [Recipient] WHERE id = {recipientId}", DBMapper.recipientMapper);

                return Created($"/recipient/get/{recipientId}", newRecipient);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }


        [HttpGet]
        public IActionResult GetRecipients(int user_id)
        {
            try
            {
                var query = $"SELECT * FROM [Recipient] WHERE user_id = ${user_id}";

                var recipients = _dbConnectionClass.GetEntities<Recipient>(query, DBMapper.recipientMapper);

                return Ok(recipients);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }
    }
}
