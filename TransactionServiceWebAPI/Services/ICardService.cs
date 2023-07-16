using TransactionServiceWebAPI.Models;

namespace TransactionServiceWebAPI.Services
{
    public interface ICardService
    {
        List<Card> Get();
        Card Get(string id);
        Card Create(Card card);
        void Update(string id, Card card);
        void Remove(string id);

        Card Get(string cardNumber, string cardholderName, int cvv);
    }
}
