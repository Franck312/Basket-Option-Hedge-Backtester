using CsvHelper;
using System.Globalization;

public class Result
{

    public static void WriteValues(string pathTh, string pathPf, (List<Record> valuesTh, List<Record> valuesPf) values)
    {

        using (var writer = new StreamWriter(pathTh))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(values.valuesTh);
        }

        using (var writer = new StreamWriter(pathPf))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(values.valuesPf);
        }

    }
}

public class Record
{
    public DateTime DateTime { get; set; }
    public double Value { get; set; }
    public Record(DateTime datetime, double value)
    {
        this.DateTime = datetime;
        this.Value = value;
    }
}

