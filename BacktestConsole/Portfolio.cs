using PricingLibrary.MarketDataFeed;

public class Portfolio
{
    public Dictionary<string, double> Underlyings { get; private set; }
    public double Cash { get; private set; }
    public DateTime LastRebalancing { get; private set; }


    public Portfolio(double value, Dictionary<string, double> underlyings, DataFeed dataFeed)
    {
        Cash = value;
        this.Underlyings = underlyings;
        foreach (var id in underlyings.Keys)
        {
            Cash -= underlyings[id] * dataFeed.PriceList[id];
        }
        LastRebalancing = dataFeed.Date;

    }


    public double UpdateValue(DataFeed dataFeed)

    {
        var value = Cash * RiskFreeRateProvider.GetRiskFreeRateAccruedValue(LastRebalancing, dataFeed.Date);

        foreach (string id in Underlyings.Keys)
        {
            value += Underlyings[id] * dataFeed.PriceList[id];
        }

        return value;
    }

    public void UpdatePositions(Dictionary<string, double> underlyings, DataFeed dataFeed, double value)
    {

        Cash = value;

        this.Underlyings = underlyings;

        foreach (string id in underlyings.Keys)
        {
            Cash -= underlyings[id] * dataFeed.PriceList[id];
        }

        LastRebalancing = dataFeed.Date;

    }

}



