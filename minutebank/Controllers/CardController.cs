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
    public class CardController : ControllerBase
    {
        private readonly IDbConnectionClass _dbConnectionClass;

        public CardController(IDbConnectionClass dbConnectionClass)
        {
            _dbConnectionClass = dbConnectionClass;
        }

        public static string GenerateRandomNumber(int total = 16)
        {
            // Create a random number generator
            Random random = new Random();

            // Generate a 24-digit account number
            string accountNumber = string.Empty;
            for (int i = 0; i < total; i++)
            {
                accountNumber += random.Next(0, 10).ToString();
            }

            return accountNumber;
        }



        [HttpPost]
        public IActionResult CreateCard([FromBody] Card card)
        {
            try
            {
                card.card_number = GenerateRandomNumber();
                card.expiry = DateTime.Now.AddYears(5);
                card.cvc = GenerateRandomNumber(3);

                var parameters = new Dictionary<string, object>
                    {
                        { "@card_number",card.card_number },
                        { "@expiry",card.expiry },
                        { "@cvc", card.cvc },
                        { "@account_id", card.account_id },
                    };

                var paramCollection = parameters.Keys.Aggregate((acc, key) => { acc = acc + ',' + key; return acc; });

                string insertQuery = $"INSERT INTO [Card] VALUES ({paramCollection})";

                var cardId = _dbConnectionClass.AddEntity<Card>(insertQuery, parameters);

                var newCard = _dbConnectionClass.GetEntities<Card>($"SELECT * FROM [Card] WHERE id = {cardId}", DBMapper.cardMapper);

                return Created($"/account/get/{cardId}", newCard);
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Something wrong" });
                throw;
            }
        }


        [HttpGet]
        public IActionResult GetCards(int user_id)
        {
            try
            {
                var query = $"SELECT Card.id as id, Card.card_number as card_number, Card.expiry as expiry, Card.cvc as cvc, Card.account_id as account_id FROM [Card] INNER JOIN Account ON Account.id = Card.account_id INNER JOIN [User] ON [User].id = Account.user_id WHERE user_id = {user_id}";

                var accounts = _dbConnectionClass.GetEntities<Card>(query, DBMapper.cardMapper);

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
