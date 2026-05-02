using FinancingApp.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace FinancingApp.Domain
{
    public class CategoryService : ICategoryService
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
            _categories.Add(new Category() { Name = categoryName });
        }

        public async Task LoadCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            _categories.Clear();
            foreach (var cagetory in categories)
            {
                _categories.Add(cagetory);
            }
        }

        public async Task<Category> FindOrCreateCategoryAsync(string name)
        {
            var existing = _categories.FirstOrDefault(c =>
                string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
            if (existing is not null) return existing;

            var category = new Category { Name = name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            _categories.Add(category);
            return category;
        }

        public ReadOnlyObservableCollection<Category> AvailableCategories { get; }
    }
}
