using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.VipLevels.Cycle.Core.ApplicationServices;
using Tgs.VipLevelService.Clients;
using Tgs.VipLevelService.Common;
using Tgs.VipLevelService.Common.ViewModels;

namespace Tgs.VipLevels.Cycle.Core.UnitTest.ApplicationServices;
public class FormPendingCommandServiceUnitTest
{
	[Fact]
	public async void Normal_Process()
	{
		var fakeLog = NullLogger<FormPendingCommandService>.Instance;
		var fakeVipLevelServiceClient = Substitute.For<IVipLevelServiceClient>();

		var command = new FormPendingCommand(
			Id: Guid.NewGuid()
		);

		var form = new PlayerVipLevelCycleFormViewModel(
			Id: command.Id,
			BrandId: Guid.NewGuid(),
			PlayerCode: "4ucmqplxb029",
			VipLevel: 1,
			CycleType: PlayerVipLevelCycleFormCycleType.Season,
			CycleOn: DateOnly.Parse("2024-01-01"),
			FromOn: DateOnly.Parse("2023-10-01"),
			ToOn: DateOnly.Parse("2024-01-01"),
			State: PlayerVipLevelCycleFormState.Create,
			Amount: 100,
			ActualAmount: 0.0m,
			Remark: string.Empty,
			CreatedOn: DateTimeOffset.UtcNow,
			CreatedUserId: Guid.Empty,
			CreatedUsername: string.Empty,
			UpdatedOn: DateTimeOffset.UtcNow,
			UpdatedUserId: Guid.Empty,
			UpdatedUsername: string.Empty);
		_ = fakeVipLevelServiceClient.PlayerVipLevelCycleFormGetAsync(
			id: Arg.Is(command.Id),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(form);

		var sut = new FormPendingCommandService(
			fakeLog,
			fakeVipLevelServiceClient);

		await sut.ExecuteAsync(command);

		_ = fakeVipLevelServiceClient
			.Received(1)
			.PlayerVipLevelCycleFormPendingAsync(
			id: Arg.Is(form.Id),
			operatorUserId: Arg.Is(Guid.Empty),
			operatorUsername: Arg.Is(" "),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
