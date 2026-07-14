namespace GDUTClassHelper.Core.Common.Type;

public class PeriodCollection
{
    public List<Period> Periods = [];

    public bool Check()
    {
        TimeOnly prev = TimeOnly.MinValue;
        foreach (var period in Periods)
        {
            if (period.StartTime < prev || period.EndTime < period.StartTime)
            {
                return false;
            }
            prev = period.EndTime;
        }
        return true;
    }
}

public struct Period
{
    public TimeOnly StartTime;
    public TimeOnly EndTime;
    public readonly TimeSpan Duration => EndTime - StartTime;
}
