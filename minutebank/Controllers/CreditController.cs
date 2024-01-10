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
    public class CreditController : ControllerBase
    {
        private readonly IDbConnectionClass _dbConnectionClass;

        public CreditController(IDbConnectionClass dbConnectionClass)
        {
            _dbConnectionClass = dbConnectionClass;
        }

        [HttpPost]
        public IActionResult CreateCredit([FromBody] Credit credit)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                    {
                        { "@date", credit.date },
                        { "@amount" , credit.amount },
                        { "@from_account_number" , credit.from_account_number is null ? DBNull.Value :  credit.from_account_number},
                        { "@payment_request_id", credit.payment_request_id is null ? DBNull.Value : credit.payment_request_id },
                        { "@account_id", credit.account_id },
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [Credit] VALUES ({paramCollection})";

                var creditId = _dbConnectionClass.AddEntity<Credit>(insertQuery, parameters);

                var newCredit = _dbConnectionClass.GetEntities<Credit>($"SELECT * FROM [Credit] WHERE id = {creditId}", DBMapper.creditMapper);

                return Created($"/credit/get/{creditId}", newCredit);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }

        [HttpPost("payrequest")]
        public IActionResult ProcessPaymentRequest([FromBody] Credit credit)
        {
            try
            {
                var paymentRequestId = credit.payment_request_id.GetValueOrDefault();

                if (paymentRequestId == 0)
                {
                    return BadRequest();
                }


                var paymentRequest = _dbConnectionClass.GetEntity<PaymentRequest>($"SELECT * FROM [PaymentRequest] WHERE id = {paymentRequestId}",DBMapper.paymentRequestMapper);

                if(paymentRequest == null) { return NotFound(); }

                var account = _dbConnectionClass.GetEntity<Account>($"SELECT * FROM [Account] WHERE id = {credit.account_id}", DBMapper.accountMapper);

                if(account == null) { return NotFound(); }

                var parameters = new Dictionary<string, object>
                    {
                        { "@date", DateTime.Now },
                        { "@amount" , paymentRequest.amount },
                        { "@from_account_number" , paymentRequest.account_number},
                        { "@payment_request_id", paymentRequest.id },
                        { "@account_id", account.id },
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [Credit] VALUES ({paramCollection})";

                var creditId = _dbConnectionClass.AddEntity<Credit>(insertQuery, parameters);

                _dbConnectionClass.UpdateEntity<PaymentRequest>($"UPDATE PaymentRequest SET status = 1 WHERE id = ${paymentRequest.id}");
                _dbConnectionClass.UpdateEntity<Account>($"UPDATE [Account] SET balance = {account.balance + paymentRequest.amount} WHERE id = {account.id}");

                var newCredit = _dbConnectionClass.GetEntity<Credit>($"SELECT * FROM [Credit] WHERE id = {creditId}", DBMapper.creditMapper);

                return Created($"/credit/get/{creditId}", newCredit);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }
    }
}
