using MongoDB.Driver;
using TransactionServiceWebAPI.Data;
using TransactionServiceWebAPI.Models;

namespace TransactionServiceWebAPI.Services
{
    public class CardService : ICardService
    {
        private readonly IMongoCollection<Card> _cards;

        public CardService(ITransactionWebApiDbSettings settings,IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _cards = database.GetCollection<Card>(settings.CardsCollectionName);
        }
        public Card Create(Card card)
        {
            _cards.InsertOne(card);
            return card;
        }

        public List<Card> Get()
        {
            return _cards.Find(card => true).ToList();
        }

        public Card Get(string id)
        {
            return _cards.Find(card => card.Id == id).FirstOrDefault();
        }
        public Card Get(string cardNumber, string cardholderName,int cvv)
        {
            return _cards.Find(card => card.CardNumber == cardNumber && card.CardholderName == cardholderName && card.CVVCode == cvv).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _cards.DeleteOne(card => card.Id == id);
        }

        public void Update(string id, Card card)
        {
            _cards.ReplaceOne(card => card.Id == id, card);
        }
    }
}
