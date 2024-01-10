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
    public class PaymentRequestController : ControllerBase
    {
        private readonly IDbConnectionClass _dbConnectionClass;

        public PaymentRequestController(IDbConnectionClass dbConnectionClass)
        {
            _dbConnectionClass = dbConnectionClass;
        }



        [HttpPost]
        public IActionResult CreatePaymentRequest([FromBody] PaymentRequest paymentRequest)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                    {
                        { "@sent_to", paymentRequest.sent_to },
                        { "@amount", paymentRequest.amount },
                        { "@due_by", paymentRequest.due_by },
                        { "@status", paymentRequest.status },
                        { "@account_number", paymentRequest.account_number},
                        { "@user_id", paymentRequest.user_id},
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [PaymentRequest] VALUES ({paramCollection})";

                var paymentRequestId = _dbConnectionClass.AddEntity<PaymentRequest>(insertQuery, parameters);

                var newPaymentRequest = _dbConnectionClass.GetEntities<PaymentRequest>($"SELECT * FROM [PaymentRequest] WHERE id = {paymentRequestId}", DBMapper.paymentRequestMapper);

                return Created($"/paymentrequest/get/{paymentRequestId}", newPaymentRequest);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }


        [HttpGet]
        public IActionResult GetPaymentRequests(int user_id)
        {
            try
            {
                var query = $"SELECT * FROM [PaymentRequest] WHERE user_id = {user_id}";

                var paymentRequests = _dbConnectionClass.GetEntities<PaymentRequest>(query, DBMapper.paymentRequestMapper);

                return Ok(paymentRequests);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }

    }
}
