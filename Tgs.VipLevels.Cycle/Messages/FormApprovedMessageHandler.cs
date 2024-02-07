using Tgs.ServiceUtility;
using Valhalla.MessageQueue;
using Tgs.VipLevels.Cycle.Core.ApplicationServices;
using Tgs.VipLevels.Cycle.Core.Protos;

namespace Tgs.VipLevels.Cycle.Messages;

internal class FormApprovedMessageHandler(
	ILogger<FormPendingMessageHandler> logger,
	ICommandService<FormApprovedCommand> service) : IMessageHandler<VipLevelsCycleFormMessage>
{
	public ValueTask HandleAsync(string subject, VipLevelsCycleFormMessage data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("LogOn:{logOn} Subject:{subject} Mesaage:This message has been received.({data})",
			DateTime.Now.ToString("o"),
			subject,
			data);

		return service.ExecuteAsync(
			command: new FormApprovedCommand(
				Id: data.Id.ToGuid()),
			cancellationToken: cancellationToken);
	}
}
