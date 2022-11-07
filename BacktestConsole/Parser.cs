using CsvHelper.Configuration;
using CsvHelper;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using System.Globalization;
using System.Text.Json;
using PricingLibrary.RebalancingOracleDescriptions;

class Parser
{

    public static TestParameters ParseTestParameters(string fileName)
    {
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { Converters = { new RebalancingOracleDescriptionConverter() } };
        string jsonString = File.ReadAllText(fileName);
        TestParameters parameters = JsonSerializer.Deserialize<TestParameters>(jsonString, jsonSerializerOptions)!;

        return parameters;
    }
    public static List<DataFeed> ParseMarketData(string fileName)
    {
        List<ShareValue> shareValues;
        Dictionary<DateTime, Dictionary<string, double>> dataFeedInput;
        List<DataFeed> dataFeeds = new List<DataFeed>();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
        };
        using (var reader = new StreamReader(fileName))
        using (var csv = new CsvReader(reader, config))
        {
            shareValues = csv.GetRecords<ShareValue>().ToList();

        }

        dataFeedInput = shareValues.GroupBy(x => x.DateOfPrice).ToDictionary(gdc => gdc.Key, gdc => gdc.ToDictionary(test => test.Id, test => test.Value));

        foreach (DateTime key in dataFeedInput.Keys)
        {
            dataFeeds.Add(new DataFeed(key, dataFeedInput[key]));
        }

        return dataFeeds;
    }

}



