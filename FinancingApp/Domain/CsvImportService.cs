using CsvHelper;
using CsvHelper.Configuration;
using FinancingApp.Common;
using System.Globalization;
using System.IO;

namespace FinancingApp.Domain
{
    public class CsvImportService(ICategoryService categoryService, ITransactionService transactionService)
    {
        private const string ChfPrefix = "CHF ";

        public async Task<(int Imported, int Skipped)> ImportAsync(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                TrimOptions = TrimOptions.Trim,
                HeaderValidated = null,
                MissingFieldFound = null,
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            int imported = 0, skipped = 0;

            await foreach (var row in csv.GetRecordsAsync<CsvRow>())
            {
                if (!DateTime.TryParseExact(row.Datum, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var date))
                {
                    skipped++;
                    continue;
                }

                Currency.TryParse(NormalizeAmount(row.Betrag), out var amount);

                var category = await categoryService.FindOrCreateCategoryAsync(row.Kategorie);
                await transactionService.CreateTransactionAsync(new Transaction
                {
                    Type = category,
                    Description = row.Posten,
                    Date = DateConverter.ToString(date),
                    Amount = amount.Value()
                });
                imported++;
            }

            return (imported, skipped);
        }

        private static string NormalizeAmount(string betrag)
        {
            var value = betrag.StartsWith(ChfPrefix)
                ? betrag[ChfPrefix.Length..].Trim()
                : betrag.Trim();

            return value is "-" or "" ? "CHF 0" : betrag;
        }

        private sealed class CsvRow
        {
            public string Datum { get; set; } = string.Empty;
            public string Posten { get; set; } = string.Empty;
            public string Kategorie { get; set; } = string.Empty;
            public string Betrag { get; set; } = string.Empty;
            // Monat is intentionally not mapped
        }
    }
}
