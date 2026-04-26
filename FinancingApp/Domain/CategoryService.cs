using FinancingApp.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace FinancingApp.Domain
{
    public class CategoryService
    {
        private readonly ObservableCollection<Category> _categories = [];

        private readonly FinancingAppContext _context;

        public CategoryService(FinancingAppContext context)
        {
            AvailableCategories = new ReadOnlyObservableCollection<Category>(_categories);
            _context = context;
        }

        public void AddCategory(string categoryName)
        {
            _categories.Add(new Category(){Name = categoryName});
        }

        public async Task LoadCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            _categories.Clear();
            foreach(var cagetory in categories)
            {
                _categories.Add(cagetory);
            }
        }

        public ReadOnlyObservableCollection<Category> AvailableCategories { get; }
    }
}
