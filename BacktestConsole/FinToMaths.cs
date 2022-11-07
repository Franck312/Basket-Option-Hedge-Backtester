using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.TimeHandler;

public class FinToMaths
{
    public static (double, double[]) PricingInput(DataFeed dataFeed, TestParameters testParameters)
    {
        double[] spots = new double[testParameters.BasketOption.UnderlyingShareIds.Length];

        for (int i = 0; i < testParameters.BasketOption.UnderlyingShareIds.Length; i++)
        {
            spots[i] = dataFeed.PriceList[testParameters.BasketOption.UnderlyingShareIds[i]];
        }

        (double, double[]) pricerInput = (MathDateConverter.ConvertToMathDistance(dataFeed.Date, testParameters.BasketOption.Maturity), spots);
        return pricerInput;
    }
    public static Dictionary<string, double> PositionsInput(PricingResults pricingResult, TestParameters testParameters)
    {

        var deltas = new Dictionary<string, double>();

        for (int i = 0; i < pricingResult.Deltas.Length; i++)
        {
            deltas[testParameters.BasketOption.UnderlyingShareIds[i]] = pricingResult.Deltas[i];
        }

        return deltas;
    }


}
