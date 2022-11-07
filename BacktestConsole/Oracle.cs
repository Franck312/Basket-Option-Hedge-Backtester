using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;
using PricingLibrary.TimeHandler;

public interface IRebOracle
{
    public Boolean IsNeeded(DataFeed dataFeed);

}

class RegularOracle : IRebOracle
{

    public int Period { get; private set; }
    public DateTime StartDate { get; private set; }

    public RegularOracle(DataFeed dataFeed, TestParameters testParameters)
    {
        this.StartDate = dataFeed.Date;
        this.Period = ((RegularOracleDescription)(testParameters.RebalancingOracleDescription)).Period;

    }
    public Boolean IsNeeded(DataFeed dataFeed)
    {
        return (int)(MathDateConverter.ConvertToMathDistance(StartDate, dataFeed.Date)*MathDateConverter.WorkingDaysPerYear) % Period == 0;
    }

}

class WeeklyOracle : IRebOracle
{
    public DayOfWeek dayOfRebalance;
    public DateTime startDate;
    public WeeklyOracle(DataFeed dataFeed, TestParameters testParameters)
    {
        this.startDate = dataFeed.Date;
        this.dayOfRebalance = ((WeeklyOracleDescription)(testParameters.RebalancingOracleDescription)).RebalancingDay;
    }

    public Boolean IsNeeded(DataFeed dataFeed)
    {
        return dataFeed.Date.DayOfWeek == dayOfRebalance;
    }
}
