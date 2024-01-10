using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minutebank.Base;
using minutebank.Mapper;
using minutebank.Models;

namespace minutebank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitController : ControllerBase
    {
        private readonly IDbConnectionClass _dbConnectionClass;

        public DebitController(IDbConnectionClass dbConnectionClass)
        {
            _dbConnectionClass = dbConnectionClass;
        }
        [HttpPost]
        public IActionResult CreateDebit([FromBody] Debit debit)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                    {
                        { "@date",debit.date },
                        { "@amount" , debit.amount },
                        { "@to_account_number" , debit.to_account_number is null ? DBNull.Value :  debit.to_account_number},
                        { "@recipient_id" , debit.recipient_id is null ? DBNull.Value : debit.recipient_id },
                        { "@account_id", debit.account_id },
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [Debit] VALUES ({paramCollection})";

                var debitId = _dbConnectionClass.AddEntity<Debit>(insertQuery, parameters);

                var newDebit = _dbConnectionClass.GetEntities<Debit>($"SELECT * FROM [Debit] WHERE id = {debitId}", DBMapper.debitMapper);

                return Created($"/debit/get/{debitId}", newDebit);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }

        [HttpPost("pay")]
        public IActionResult PayRecipient([FromBody] Debit debit)
        {
            try
            {
                var recipientId = debit.recipient_id.GetValueOrDefault();

                if (recipientId == 0)
                {
                    return BadRequest();
                }

                var recipient = _dbConnectionClass.GetEntity<Recipient>($"SELECT * FROM [Recipient] WHERE id = {recipientId}", DBMapper.recipientMapper);

                if (recipient == null) { return NotFound(); }

                var account = _dbConnectionClass.GetEntity<Account>($"SELECT * FROM [Account] WHERE id = {debit.account_id}", DBMapper.accountMapper);

                if (account == null) { return NotFound(); }

                if(debit.amount > account.balance) return BadRequest();

                var parameters = new Dictionary<string, object>
                    {
                        { "@date",DateTime.Now },
                        { "@amount" , debit.amount },
                        { "@recipient_id" , recipient.id },
                        { "@account_id", account.id },
                        { "@to_account_number" , account.account_number},
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [Debit] VALUES ({paramCollection})";

                var debitId = _dbConnectionClass.AddEntity<Debit>(insertQuery, parameters);

                _dbConnectionClass.UpdateEntity<Account>($"UPDATE [Account] SET balance = {account.balance - debit.amount} WHERE id = {account.id}");

                var newDebit = _dbConnectionClass.GetEntity<Debit>($"SELECT * FROM [Debit] WHERE id = {debitId}", DBMapper.debitMapper);

                return Created($"/debit/get/{debitId}", newDebit);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }
    }
}
