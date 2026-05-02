namespace FinancingApp.Domain;

public interface ICategoryService
{
    Task<Category> FindOrCreateCategoryAsync(string name);
}
