using System.Collections.ObjectModel;

namespace FinancingApp.Domain
{
    public class CategoryService
    {
        private readonly ObservableCollection<Category> _categories = [];

        public CategoryService()
        {
            AvailableCategories = new ReadOnlyObservableCollection<Category>(_categories);

            // dummy data
            _categories.Add(new Category(){Name = "Kat. AAAAAAA"});
            _categories.Add(new Category(){Name = "Kat. BBBBBBB"});
        }

        public void AddCategory(string categoryName)
        {
            _categories.Add(new Category(){Name = categoryName});
        }

        public ReadOnlyObservableCollection<Category> AvailableCategories { get; }
    }
}
