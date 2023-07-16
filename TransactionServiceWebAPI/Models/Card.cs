using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransactionServiceWebAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Card
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("card_number")]
        public string CardNumber { get; set; }
        [BsonElement("expiration_date")]
        public DateTime ExpirationDate { get; set; }
        [BsonElement("cvv_code")]
        public int CVVCode { get; set; }
        [BsonElement("cardholder_name")]
        public string CardholderName { get; set; }

        [BsonElement("balance")]
        public double Balance { get; set; }
    }

}
