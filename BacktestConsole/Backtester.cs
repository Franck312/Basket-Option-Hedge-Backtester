using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;
using PricingLibrary.TimeHandler;

public class Backtester
{
    public Portfolio? Portfolio { get; private set; }
    public IRebOracle? Oracle { get; private set; }
    public TestParameters TestParameters { get; private set; }
    public Pricer Pricer { get; private set; }
    public Dictionary<string, double>? Deltas { get; private set; }
    public List<Record> ValuesTh { get; private set; }
    public List<Record> ValuesPf { get; private set; }

    public Backtester(TestParameters testParameters)
    {
        ValuesTh = new List<Record>();
        ValuesPf = new List<Record>();
        this.TestParameters = testParameters;
        Pricer = new Pricer(testParameters);

    }

    public (List<Record>, List<Record>) Run(List<DataFeed> dataFeeds)
    {

        var timeToMaturity = MathDateConverter.ConvertToMathDistance(dataFeeds[0].Date, TestParameters.BasketOption.Maturity);
        double[] spots = dataFeeds[0].PriceList.Values.ToArray();
        PricingResults pricingResult = Pricer.Price(timeToMaturity, spots);
        double value = pricingResult.Price;
        Deltas = new Dictionary<string, double>();

        Deltas = FinToMaths.PositionsInput(pricingResult, TestParameters);
        Portfolio = new Portfolio(pricingResult.Price, Deltas, dataFeeds[0]);

        switch (TestParameters.RebalancingOracleDescription.Type)
        {
            case RebalancingOracleType.Weekly:
                Oracle = new WeeklyOracle(dataFeeds[0], TestParameters);
                break;
            case RebalancingOracleType.Regular:
                Oracle = new RegularOracle(dataFeeds[0], TestParameters);
                break;
            default: throw new Exception();
        }

        ValuesPf.Add(new Record(dataFeeds[0].Date, value));
        ValuesTh.Add(new Record(dataFeeds[0].Date, pricingResult.Price));

        foreach (DataFeed dataFeed in dataFeeds.Skip(1))
        {

            (timeToMaturity, spots) = FinToMaths.PricingInput(dataFeed, TestParameters);

            pricingResult = Pricer.Price(timeToMaturity, spots);

            value = Portfolio.UpdateValue(dataFeed);

            ValuesPf.Add(new Record(dataFeed.Date, value));
            ValuesTh.Add(new Record(dataFeed.Date, pricingResult.Price));

            if (Oracle.IsNeeded(dataFeed))
            {
                Deltas = FinToMaths.PositionsInput(pricingResult, TestParameters);
                Portfolio.UpdatePositions(Deltas, dataFeed, value);
            }
        }

        return (ValuesTh, ValuesPf);
    }

}

