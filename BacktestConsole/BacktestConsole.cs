using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;


public class BacktestConsole
{
    public static void Main(string[] args)
    {
        TestParameters testParameters = Parser.ParseTestParameters(args[0]);
        List<DataFeed> dataFeeds = Parser.ParseMarketData(args[1]);

        Backtester backtester = new Backtester(testParameters);
        (List<Record> valuesTh, List<Record> valuesPf) values = backtester.Run(dataFeeds);

        Result.WriteValues(args[3], args[2], values);

    }
}

