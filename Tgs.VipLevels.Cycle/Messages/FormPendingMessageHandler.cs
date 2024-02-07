using Tgs.ServiceUtility;
using Valhalla.MessageQueue;
using Tgs.VipLevels.Cycle.Core.Protos;
using Tgs.VipLevels.Cycle.Core.ApplicationServices;

namespace Tgs.VipLevels.Cycle.Messages;

internal class FormPendingMessageHandler(
	ILogger<FormPendingMessageHandler> logger,
	ICommandService<FormPendingCommand> service) : IMessageHandler<VipLevelsCycleFormMessage>
{
	public ValueTask HandleAsync(string subject, VipLevelsCycleFormMessage data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("LogOn:{logOn} Subject:{subject} Mesaage:This message has been received.({data})",
			DateTime.Now.ToString("o"),
			subject,
			data);

		return service.ExecuteAsync(
			command: new FormPendingCommand(
				Id: data.Id.ToGuid()),
			cancellationToken: cancellationToken);
	}
}
