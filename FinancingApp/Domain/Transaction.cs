using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancingApp.Domain
{
    public class Transaction
    {
        /// <summary>
        /// Needed by Entity Framework, but unused in the application.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; private set; }

        public Category Type { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// The date is stored as yyyy-mm-dd
        /// </summary>
        [StringLength(10)]
        public string Date { get; set; }

        public int Amount { get; set; }
    }
}
