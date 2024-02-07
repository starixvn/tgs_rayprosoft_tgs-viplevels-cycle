namespace Tgs.VipLevels.Cycle.Core;

public interface ISystemClock
{
	DateTimeOffset CurrentTime { get; }

	(DateTimeOffset FromOn, DateTimeOffset ToOn) GetSeasonTimeZone(DateTimeOffset source, int timeSpanMinutes = 480);

	DateTimeOffset GetTimeZone(DateTimeOffset source, int timeSpanMinutes = 480);

	DateTimeOffset GetDayTimeZone(DateTimeOffset source, int timeSpanMinutes = 480);

	(DateTimeOffset FromOn, DateTimeOffset ToOn) GetMonthsTimeZone(DateTimeOffset source, int timeSpanMinutes = 480);
}