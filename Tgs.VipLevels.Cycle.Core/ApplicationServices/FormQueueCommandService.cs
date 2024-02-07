using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;
using Tgs.VipLevelService.Clients;
using Valhalla.MessageQueue;
using Tgs.VipLevels.Cycle.Core.Protos;

namespace Tgs.VipLevels.Cycle.Core.ApplicationServices;
internal class FormQueueCommandService(
	ILogger<FormQueueCommandService> logger,
	IVipLevelServiceClient vipLevelServiceClient,
	IMessageSender messageSender) : ICommandService<FormQueueCommand>
{
	public async ValueTask ExecuteAsync(FormQueueCommand command, CancellationToken cancellationToken = default)
	{
		var form = await vipLevelServiceClient.PlayerVipLevelBirthdayFormGetAsync(
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

		if (form.State != VipLevelService.Common.PlayerVipLevelBirthdayFormState.Create)
		{
			logger.LogWarning("LogOn:{logOn} Message:This form({id}) status is not created.({state})",
				DateTime.Now.ToString("u"),
				form.Id,
				form.State);
			return;
		}

		/*
		 * 判斷是否自動還是人工
		 */
		if (true)
		{
			var message = new VipLevelsCycleFormMessage
			{
				Id = form.Id
			};
			await messageSender.PublishAsync(
				subject: VipLevelsCycleNatsConst.Approved,
				data: message,
				cancellationToken: cancellationToken)
				.ConfigureAwait(false);

			logger.LogInformation("LogOn:{logOn} Subject:{subject} Message:This message has been published.({message})",
				DateTime.Now.ToString("u"),
				VipLevelsCycleNatsConst.Approved,
				message);
		}
	}
}
