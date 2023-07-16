namespace TransactionServiceWebAPI.Data
{
    public interface ITransactionWebApiDbSettings
    {
        string CardsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
