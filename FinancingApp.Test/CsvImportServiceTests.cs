using FinancingApp.Domain;
using Moq;

namespace FinancingApp.Test
{
    [TestFixture]
    public class CsvImportServiceTests
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private Mock<ITransactionService> _transactionServiceMock;
        private CsvImportService _sut;

        private const string CsvHeader = "Datum;Posten;Kategorie; Betrag ;Monat";

        [SetUp]
        public void SetUp()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _transactionServiceMock = new Mock<ITransactionService>();

            _categoryServiceMock
                .Setup(s => s.FindOrCreateCategoryAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => new Category { Name = name });

            _transactionServiceMock
                .Setup(s => s.CreateTransactionAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            _sut = new CsvImportService(_categoryServiceMock.Object, _transactionServiceMock.Object);
        }

        [Test]
        public async Task ValidRow_ImportsOneTransaction()
        {
            var path = await WriteTempCsvAsync("01.10.2024;Migros;Lebenshaltung; CHF 20.15 ;10");

            var (imported, skipped) = await _sut.ImportAsync(path);

            Assert.That(imported, Is.EqualTo(1));
            Assert.That(skipped, Is.EqualTo(0));
        }

        [Test]
        public async Task InvalidDate_SkipsRow()
        {
            var path = await WriteTempCsvAsync("32.13.2024;Bad;Cat; CHF 1.00 ;1");

            var (imported, skipped) = await _sut.ImportAsync(path);

            Assert.That(imported, Is.EqualTo(0));
            Assert.That(skipped, Is.EqualTo(1));
        }

        [Test]
        public async Task HeaderOnly_ReturnsZeroCounts()
        {
            var path = await WriteTempCsvAsync(string.Empty);

            var (imported, skipped) = await _sut.ImportAsync(path);

            Assert.That(imported, Is.EqualTo(0));
            Assert.That(skipped, Is.EqualTo(0));
        }

        [Test]
        public async Task ChfDash_ImportsAsZeroAmount()
        {
            var path = await WriteTempCsvAsync("01.10.2024;Dummy;Sonstiges; CHF -   ;10");
            Transaction? captured = null;
            _transactionServiceMock
                .Setup(s => s.CreateTransactionAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(t => captured = t)
                .Returns(Task.CompletedTask);

            await _sut.ImportAsync(path);

            Assert.That(captured, Is.Not.Null);
            Assert.That(captured!.Amount, Is.EqualTo(0));
        }

        [Test]
        public async Task ApostropheSeparator_ParsesCorrectly()
        {
            var path = await WriteTempCsvAsync("07.10.2024;Steuern;Steuern; CHF 10'000.00 ;10");
            Transaction? captured = null;
            _transactionServiceMock
                .Setup(s => s.CreateTransactionAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(t => captured = t)
                .Returns(Task.CompletedTask);

            await _sut.ImportAsync(path);

            Assert.That(captured, Is.Not.Null);
            Assert.That(captured!.Amount, Is.EqualTo(1_000_000));
        }

        [Test]
        public async Task ValidRow_CallsFindOrCreateWithCategoryName()
        {
            var path = await WriteTempCsvAsync("01.10.2024;Crunchyroll;Abos; CHF 8.90 ;10");

            await _sut.ImportAsync(path);

            _categoryServiceMock.Verify(s => s.FindOrCreateCategoryAsync("Abos"), Times.Once);
        }

        [Test]
        public async Task MixedRows_CountsBothCorrectly()
        {
            var body = string.Join("\n", [
                "01.10.2024;Migros;Lebenshaltung; CHF 20.15 ;10",
                "32.13.2024;Bad;Cat; CHF 1.00 ;1",
                "02.10.2024;Volg;Lebenshaltung; CHF 16.10 ;10",
            ]);
            var path = await WriteTempCsvAsync(body);

            var (imported, skipped) = await _sut.ImportAsync(path);

            Assert.That(imported, Is.EqualTo(2));
            Assert.That(skipped, Is.EqualTo(1));
        }

        private static async Task<string> WriteTempCsvAsync(string body)
        {
            var path = Path.GetTempFileName();
            var content = string.IsNullOrEmpty(body) ? CsvHeader : CsvHeader + "\n" + body;
            await File.WriteAllTextAsync(path, content);
            return path;
        }
    }
}
