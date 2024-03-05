namespace Tgs.VipLevels.Cycle.Core;

internal class DateTimeUtensil : IDateTimeUtensil
{
	public DateTimeOffset GetDayTimeZone(DateTimeOffset source, int timeSpanMinutes = 480)
	{
		var timeZone = GetTimeZone(source, timeSpanMinutes);
		return new DateTimeOffset(timeZone.Year, timeZone.Month, timeZone.Day, 00, 00, 00, timeZone.Offset);
	}

	public (DateTimeOffset FromOn, DateTimeOffset ToOn) GetMonthsTimeZone(DateTimeOffset source, int timeSpanMinutes = 480)
	{
		var day = GetDayTimeZone(source, timeSpanMinutes);

		var fromOn = new DateTimeOffset(day.Year, day.Month, 1, 00, 00, 00, day.Offset);
		var toOn = fromOn.AddMonths(1);

		return new(fromOn, toOn);
	}

	public (DateTimeOffset FromOn, DateTimeOffset ToOn) GetSeasonTimeZone(DateTimeOffset source, int timeSpanMinutes = 480)
	{
		var (fromOn, toOn) = GetMonthsTimeZone(source, timeSpanMinutes);
		return toOn.Month % 3 == 1
			? (toOn.AddMonths(-3), toOn)
			: fromOn.Month % 3 == 1 && toOn.Month % 3 == 2
			? (fromOn, toOn.AddMonths(+2))
			: (fromOn.AddMonths(-1), toOn.AddMonths(+1));
	}

	public DateTimeOffset GetTimeZone(DateTimeOffset source, int timeSpanMinutes = 480)
	{
		var timeSpan = TimeSpan.FromMinutes(timeSpanMinutes);
		return source.ToOffset(timeSpan);
	}
}
