using System.Collections.ObjectModel;

namespace FinancingApp.Domain;

public interface ICategoryService
{
    void AddCategory(string categoryName);

    Task LoadCategoriesAsync();

    Task<Category> FindOrCreateCategoryAsync(string name);

    ReadOnlyObservableCollection<Category> AvailableCategories { get; }
}
