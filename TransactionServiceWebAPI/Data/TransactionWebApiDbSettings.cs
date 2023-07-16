namespace TransactionServiceWebAPI.Data
{
    public class TransactionWebApiDbSettings : ITransactionWebApiDbSettings
    {
        public string CardsCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
