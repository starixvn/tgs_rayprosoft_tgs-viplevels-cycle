using Microsoft.Extensions.Logging;
using Tgs.PlayerService.Client;
using Tgs.ServiceUtility;
using Tgs.VipLevelService.Clients;

namespace Tgs.VipLevels.Cycle.Core.ApplicationServices;

internal class FormCreateCommandService(
	ILogger<FormCreateCommandService> logger,
	ISystemClock systemClock,
	IPlayerServiceClient playerServiceClient,
	IVipLevelServiceClient vipLevelServiceClient) : ICommandService<FormCreateCommand>
{
	public async ValueTask ExecuteAsync(FormCreateCommand command, CancellationToken cancellationToken = default)
	{
		IEnumerable<string> playerCodes;
		var currentTime = systemClock.GetTimeZone(command.CurrentTime, command.TimeSpanMinutes);
		var day = systemClock.GetDayTimeZone(currentTime, command.TimeSpanMinutes);
		var yesterday = day.AddDays(-1);
		(DateTimeOffset SeasonFromOn, DateTimeOffset SeasonToOn) = systemClock.GetSeasonTimeZone(yesterday, command.TimeSpanMinutes);
		(DateTimeOffset MonthFromOn, DateTimeOffset MonthToOn) = systemClock.GetMonthsTimeZone(yesterday, command.TimeSpanMinutes);
		var compareTo = day.CompareTo(SeasonToOn);

		logger.LogInformation("LogOn:{logOn} Message:Process information in this cycle.({info})",
			DateTime.Now.ToString("o"),
			 new
			 {
				 CurrentTime = currentTime,
				 SeasonFromOn,
				 SeasonToOn,
				 MonthFromOn,
				 MonthToOn,
				 CompareTo = compareTo
			 });

		if (compareTo < 0)
		{
			logger.LogWarning("LogOn:{logOn} Message:The cycle benefit payment time has not been reached.({info})",
				DateTime.Now.ToString("o"),
				new
				{
					CurrentTime = currentTime,
					SeasonFromOn,
					SeasonToOn,
					MonthFromOn,
					MonthToOn,
					CompareTo = compareTo
				});
			return;
		}

		var cycleType = VipLevelService.Common.PlayerVipLevelCycleFormCycleType.Season;
		var cycleOn = DateOnly.FromDateTime(SeasonToOn.DateTime);
		var fromOn = DateOnly.FromDateTime(SeasonFromOn.DateTime);
		var toOn = DateOnly.FromDateTime(SeasonToOn.DateTime);
		var page = 1;
		var rows = 100;
		do
		{
			playerCodes = await playerServiceClient.QueryPlayerLoginRecordGroupPlayerAsync(
				brandId: command.BrandId,
				startOn: MonthFromOn,
				endOn: MonthToOn,
				page: page,
				rows: rows,
				cancellationToken: cancellationToken)
				.ToArrayAsync(
				cancellationToken: cancellationToken)
				.ConfigureAwait(false)
				?? Enumerable.Empty<string>();
			foreach (var playerCode in playerCodes)
			{
				if (await vipLevelServiceClient.PlayerVipLevelCycleFormExistAsync(
					playerCode: playerCode,
					cycleType: cycleType,
					cycleOn: cycleOn,
					cancellationToken: cancellationToken)
					.ConfigureAwait(false))
				{
					logger.LogWarning("LogOn:{logOn} Message:This player({playerCode}) has sent periodic benefits.({fromOn}~{toOn})",
						DateTime.Now.ToString("u"),
						playerCode,
						fromOn,
						toOn);
					continue;
				}

				var id = await vipLevelServiceClient.PlayerVipLevelCycleFormCreateAsync(
					playerCode: playerCode,
					cycleType: cycleType,
					cycleOn: cycleOn,
					fromOn: fromOn,
					toOn: toOn,
					operatorUserId: Guid.Empty,
					operatorUsername: "system",
					cancellationToken: cancellationToken)
					.ConfigureAwait(false);

				logger.LogInformation("LogOn:{logOn}  Message:This player VIP level form was created successfully.({id})",
					DateTime.Now.ToString("u"),
					id);
			}

			page++;
		} while (playerCodes.Any());
	}
}
