using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancingApp.Domain
{
    public class Category
    {
        /// <summary>
        /// Needed for Entity Framework, but unused in the application.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; private set; }
        public string Name { get; set; } = string.Empty;

        public static Category Null = new ();
    }
}
