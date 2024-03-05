using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.PlayerService.Client;
using Tgs.VipLevels.Cycle.Core;
using Tgs.VipLevels.Cycle.Core.ApplicationServices;
using Tgs.VipLevelService.Clients;

namespace Tgs.VipLevels.Birthday.Core.UnitTest.ApplicationServices;

public class FormCreateCommandServiceUnitTest
{
	private static readonly string[] _SourceArray = ["4ucmqplxb029"];

	[Fact]
	public async void Normal_Process()
	{
		var fakeLog = NullLogger<FormCreateCommandService>.Instance;
		var fakeDateTimeUtensil = Substitute.For<IDateTimeUtensil>();
		var fakePlayerServiceClient = Substitute.For<IPlayerServiceClient>();
		var fakeVipLevelServiceClient = Substitute.For<IVipLevelServiceClient>();

		var command = new FormCreateCommand
		(
			ResellerId: Guid.NewGuid(),
			ResellerCode: "Demon",
			LicenseeId: Guid.NewGuid(),
			LicenseeCode: "GPI",
			BrandId: Guid.NewGuid(),
			BrandCode: "GPI",
			TimeSpanMinutes: 480,
			CurrentTime: new DateTimeOffset(2024, 12, 31, 16, 00, 00, TimeSpan.Zero)
		);

		var timeZoneCurrentTime = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetTimeZone(
			source: Arg.Is(command.CurrentTime),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns(timeZoneCurrentTime);

		var dayTimeZone = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetDayTimeZone(
			source: Arg.Is(timeZoneCurrentTime),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns(dayTimeZone);

		var yesterday = dayTimeZone.AddDays(-1);

		var seasonFromOn = new DateTimeOffset(2023, 10, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		var seasonToOn = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetSeasonTimeZone(
			source: Arg.Is(yesterday),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns((seasonFromOn, seasonToOn));

		var monthFromOn = new DateTimeOffset(2023, 12, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		var monthToOn = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetMonthsTimeZone(
			source: Arg.Is(yesterday),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns((monthFromOn, monthToOn));

		var cycleType = VipLevelService.Common.PlayerVipLevelCycleFormCycleType.Season;
		var cycleOn = DateOnly.FromDateTime(seasonToOn.DateTime);
		var fromOn = DateOnly.FromDateTime(seasonFromOn.DateTime);
		var toOn = DateOnly.FromDateTime(seasonToOn.DateTime);

		_ = fakePlayerServiceClient.QueryPlayerLoginRecordGroupPlayerAsync(
			brandId: Arg.Is(command.BrandId),
			startOn: Arg.Is(monthFromOn),
			endOn: Arg.Is(monthToOn),
			page: Arg.Any<int>(),
			rows: Arg.Any<int>(),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(_SourceArray.ToAsyncEnumerable(), Enumerable.Empty<string>().ToAsyncEnumerable());

		_ = fakeVipLevelServiceClient.PlayerVipLevelCycleFormExistAsync(
			playerCode: Arg.Is("4ucmqplxb029"),
			cycleType: Arg.Is(cycleType),
			cycleOn: Arg.Is(cycleOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(false);

		var id = Guid.NewGuid();
		_ = fakeVipLevelServiceClient.PlayerVipLevelCycleFormCreateAsync(
			playerCode: Arg.Is("4ucmqplxb029"),
			cycleType: Arg.Is(cycleType),
			cycleOn: Arg.Is(cycleOn),
			fromOn: Arg.Is(fromOn),
			toOn: Arg.Is(toOn),
			operatorUserId: Arg.Is(Guid.Empty),
			operatorUsername: Arg.Is("system"),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(id);

		var sut = new FormCreateCommandService(
			fakeLog,
			fakeDateTimeUtensil,
			fakePlayerServiceClient,
			fakeVipLevelServiceClient);

		await sut.ExecuteAsync(command);
	}

	[Fact]
	public async void This_player_has_sent_periodic_benefits()
	{
		var fakeLog = NullLogger<FormCreateCommandService>.Instance;
		var fakeDateTimeUtensil = Substitute.For<IDateTimeUtensil>();
		var fakePlayerServiceClient = Substitute.For<IPlayerServiceClient>();
		var fakeVipLevelServiceClient = Substitute.For<IVipLevelServiceClient>();

		var command = new FormCreateCommand
		(
			ResellerId: Guid.NewGuid(),
			ResellerCode: "Demon",
			LicenseeId: Guid.NewGuid(),
			LicenseeCode: "GPI",
			BrandId: Guid.NewGuid(),
			BrandCode: "GPI",
			TimeSpanMinutes: 480,
			CurrentTime: new DateTimeOffset(2024, 12, 31, 16, 00, 00, TimeSpan.Zero)
		);

		var timeZoneCurrentTime = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetTimeZone(
			source: Arg.Is(command.CurrentTime),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns(timeZoneCurrentTime);

		var dayTimeZone = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetDayTimeZone(
			source: Arg.Is(timeZoneCurrentTime),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns(dayTimeZone);

		var yesterday = dayTimeZone.AddDays(-1);

		var seasonFromOn = new DateTimeOffset(2023, 10, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		var seasonToOn = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetSeasonTimeZone(
			source: Arg.Is(yesterday),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns((seasonFromOn, seasonToOn));

		var monthFromOn = new DateTimeOffset(2023, 12, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		var monthToOn = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetMonthsTimeZone(
			source: Arg.Is(yesterday),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns((monthFromOn, monthToOn));

		var cycleType = VipLevelService.Common.PlayerVipLevelCycleFormCycleType.Season;
		var cycleOn = DateOnly.FromDateTime(seasonToOn.DateTime);
		var fromOn = DateOnly.FromDateTime(seasonFromOn.DateTime);
		var toOn = DateOnly.FromDateTime(seasonToOn.DateTime);

		_ = fakePlayerServiceClient.QueryPlayerLoginRecordGroupPlayerAsync(
			brandId: Arg.Is(command.BrandId),
			startOn: Arg.Is(monthFromOn),
			endOn: Arg.Is(monthToOn),
			page: Arg.Any<int>(),
			rows: Arg.Any<int>(),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(_SourceArray.ToAsyncEnumerable(), Enumerable.Empty<string>().ToAsyncEnumerable());

		_ = fakeVipLevelServiceClient.PlayerVipLevelCycleFormExistAsync(
			playerCode: Arg.Is("4ucmqplxb029"),
			cycleType: Arg.Is(cycleType),
			cycleOn: Arg.Is(cycleOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(true);

		var sut = new FormCreateCommandService(
			fakeLog,
			fakeDateTimeUtensil,
			fakePlayerServiceClient,
			fakeVipLevelServiceClient);

		await sut.ExecuteAsync(command);

		_ = fakeVipLevelServiceClient
			.Received(0)
			.PlayerVipLevelCycleFormCreateAsync(
			playerCode: Arg.Is("4ucmqplxb029"),
			cycleType: Arg.Is(cycleType),
			cycleOn: Arg.Is(cycleOn),
			fromOn: Arg.Is(fromOn),
			toOn: Arg.Is(toOn),
			operatorUserId: Arg.Is(Guid.Empty),
			operatorUsername: Arg.Is("system"),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async void The_cycle_benefit_payment_time_has_not_been_reached()
	{
		var fakeLog = NullLogger<FormCreateCommandService>.Instance;
		var fakeDateTimeUtensil = Substitute.For<IDateTimeUtensil>();
		var fakePlayerServiceClient = Substitute.For<IPlayerServiceClient>();
		var fakeVipLevelServiceClient = Substitute.For<IVipLevelServiceClient>();

		var command = new FormCreateCommand
		(
			ResellerId: Guid.NewGuid(),
			ResellerCode: "Demon",
			LicenseeId: Guid.NewGuid(),
			LicenseeCode: "GPI",
			BrandId: Guid.NewGuid(),
			BrandCode: "GPI",
			TimeSpanMinutes: 480,
			CurrentTime: new DateTimeOffset(2024, 01, 01, 16, 00, 00, TimeSpan.Zero)
		);

		var timeZoneCurrentTime = new DateTimeOffset(2024, 01, 02, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetTimeZone(
			source: Arg.Is(command.CurrentTime),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns(timeZoneCurrentTime);

		var dayTimeZone = new DateTimeOffset(2024, 01, 02, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetDayTimeZone(
			source: Arg.Is(timeZoneCurrentTime),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns(dayTimeZone);

		var yesterday = dayTimeZone.AddDays(-1);

		var seasonFromOn = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		var seasonToOn = new DateTimeOffset(2024, 04, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetSeasonTimeZone(
			source: Arg.Is(yesterday),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns((seasonFromOn, seasonToOn));

		var monthFromOn = new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		var monthToOn = new DateTimeOffset(2024, 02, 01, 00, 00, 00, TimeSpan.FromMinutes(command.TimeSpanMinutes));
		_ = fakeDateTimeUtensil.GetMonthsTimeZone(
			source: Arg.Is(yesterday),
			timeSpanMinutes: Arg.Is(command.TimeSpanMinutes))
			.Returns((monthFromOn, monthToOn));

		var cycleType = VipLevelService.Common.PlayerVipLevelCycleFormCycleType.Season;
		var cycleOn = DateOnly.FromDateTime(seasonToOn.DateTime);
		var fromOn = DateOnly.FromDateTime(seasonFromOn.DateTime);
		var toOn = DateOnly.FromDateTime(seasonToOn.DateTime);

		var sut = new FormCreateCommandService(
			fakeLog,
			fakeDateTimeUtensil,
			fakePlayerServiceClient,
			fakeVipLevelServiceClient);

		await sut.ExecuteAsync(command);

		_ = fakePlayerServiceClient
			.Received(0)
			.QueryPlayerLoginRecordGroupPlayerAsync(
			brandId: Arg.Is(command.BrandId),
			startOn: Arg.Is(monthFromOn),
			endOn: Arg.Is(monthToOn),
			page: Arg.Any<int>(),
			rows: Arg.Any<int>(),
			cancellationToken: Arg.Any<CancellationToken>());

		_ = fakeVipLevelServiceClient
			.Received(0)
			.PlayerVipLevelCycleFormExistAsync(
			playerCode: Arg.Is("4ucmqplxb029"),
			cycleType: Arg.Is(cycleType),
			cycleOn: Arg.Is(cycleOn),
			cancellationToken: Arg.Any<CancellationToken>());

		_ = fakeVipLevelServiceClient
			.Received(0)
			.PlayerVipLevelCycleFormCreateAsync(
			playerCode: Arg.Is("4ucmqplxb029"),
			cycleType: Arg.Is(cycleType),
			cycleOn: Arg.Is(cycleOn),
			fromOn: Arg.Is(fromOn),
			toOn: Arg.Is(toOn),
			operatorUserId: Arg.Is(Guid.Empty),
			operatorUsername: Arg.Is("system"),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}