using Microsoft.AspNetCore.Mvc;
using TransactionServiceWebAPI.Models;
using TransactionServiceWebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransactionServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService cardService;

        public CardsController(ICardService cardService)
        {
            this.cardService = cardService;
        }
        // GET: api/<CardsController>
        [HttpGet]
        public ActionResult<List<Card>> Get()
        {
            return cardService.Get();
        }

        // GET api/<CardsController>/5
        [HttpGet("{id}")]
        public ActionResult<Card> Get(string id)
        {
            return cardService.Get(id);
        }

        // POST api/<CardsController>
        [HttpPost]
        public ActionResult<Card> Post([FromBody] Card card)
        {
            cardService.Create(card);
            return CreatedAtAction(nameof(Get), new { id = card.Id }, card);
        }

        // PUT api/<CardsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Card card)
        {
            var existingCard = cardService.Get(id);
            if (existingCard==null)
            {
                return NotFound($"Card with id = {id} not found");
            }
            cardService.Update(id, card);
            return NoContent();
        }

        // DELETE api/<CardsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var card = cardService.Get(id);
            if(card==null)
                return NotFound($"Card with id = {id} not found");

            cardService.Remove(id);

            return Ok($"Card with id = {id} deleted");
        }

        [HttpPut]
        [Route("Payment")]
        public ActionResult Payment([FromBody] CardViewModel card)
        {
            var existingCard = cardService.Get(card.CardNumber,card.CardholderName,card.CVVCode);
            if (existingCard == null)
                return NotFound("Card information does not match with an existing card");
            
            if (card.ExpireMonth!=existingCard.ExpirationDate.Month || card.ExpireYear!=existingCard.ExpirationDate.Year)
                return BadRequest("Card information does not match with an existing card");
            
            if (existingCard.ExpirationDate < DateTime.Now)
                return BadRequest("Card has expired");
            
            if (existingCard.Balance < card.AmountOfPayment)
                return BadRequest("Card has insufficient balance");
            
            existingCard.Balance -= card.AmountOfPayment;
            cardService.Update(existingCard.Id, existingCard);

            return Ok("Payment is received!");
        }
    }
}
