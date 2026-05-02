namespace FinancingApp.Domain;

public interface ITransactionService
{
    Task CreateTransactionAsync(Transaction transaction);
}
