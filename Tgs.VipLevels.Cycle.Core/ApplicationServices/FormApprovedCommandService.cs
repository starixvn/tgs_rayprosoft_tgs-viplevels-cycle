using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;
using Tgs.VipLevelService.Clients;
using Tgs.VipLevels.Cycle.Core.ApplicationServices;

namespace Tgs.VipLevels.Cycle.Core.ApplicationServices;
internal class FormApprovedCommandService(
	ILogger<FormApprovedCommandService> logger,
	IVipLevelServiceClient vipLevelServiceClient) : ICommandService<FormApprovedCommand>
{
	public async ValueTask ExecuteAsync(FormApprovedCommand command, CancellationToken cancellationToken = default)
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

		await vipLevelServiceClient.PlayerVipLevelCycleFormApprovedAsync(
			id: form.Id,
			amount: form.Amount,
			remark: "This system automatically passes the review",
			operatorUserId: Guid.Empty,
			operatorUsername: " ",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}

