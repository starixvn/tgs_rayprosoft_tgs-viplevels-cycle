using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;
using Tgs.VipLevelService.Clients;
using Tgs.VipLevels.Cycle.Core.ApplicationServices;

namespace Tgs.VipLevels.Cycle.Core.ApplicationServices;
internal class FormPendingCommandService(
	ILogger<FormPendingCommandService> logger,
	IVipLevelServiceClient vipLevelServiceClient) : ICommandService<FormPendingCommand>
{
	public async ValueTask ExecuteAsync(FormPendingCommand command, CancellationToken cancellationToken = default)
	{
		var form = await vipLevelServiceClient.PlayerVipLevelCycleFormGetAsync(
			id: command.Id,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (form is null)
		{
			logger.LogWarning("LogOn:{logOn} Message:This form does not exist.({id})",
				DateTime.Now.ToString("u"),
				command.Id);
			return;
		}

		if (form.State != VipLevelService.Common.PlayerVipLevelCycleFormState.Create)
		{
			logger.LogWarning("LogOn:{logOn} Message:This form({id}) status is not created.({state})",
				DateTime.Now.ToString("u"),
				form.Id,
				form.State);
			return;
		}

		await vipLevelServiceClient.PlayerVipLevelCycleFormPendingAsync(
			id: form.Id,
			operatorUserId: Guid.Empty,
			operatorUsername: "system",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}

