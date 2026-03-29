namespace FinancingApp.Common;

public static class DateConverter
{
    public static DateTime ToDateTime(string date)
    {
        var dateParts = date.Split('-');
        if (dateParts.Length != 3)
        {
            throw new ArgumentException($"Invalid date format: {date}. Expected format is yyyy-mm-dd.");
        }

        try
        {
            var year = int.Parse(dateParts[0]);
            var month = int.Parse(dateParts[1]);
            var day = int.Parse(dateParts[2]);

            return new DateTime(year, month, day);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Invalid date format: {date}. Expected format is yyyy-mm-dd.");
        }
    }

    public static string ToString(DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }
}