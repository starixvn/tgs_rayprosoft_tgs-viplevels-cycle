using Tgs.ServiceUtility;
using Tgs.VipLevels.Cycle.Core.ApplicationServices;
using Tgs.VipLevels.Cycle.Core.Protos;

using Valhalla.MessageQueue;

namespace Tgs.VipLevels.Cycle.Messages;

internal class FormQueueMessageHandler(
	ILogger<FormQueueMessageHandler> logger,
	ICommandService<FormQueueCommand> service) : IMessageHandler<VipLevelsCycleFormMessage>
{
	public ValueTask HandleAsync(string subject, VipLevelsCycleFormMessage data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("LogOn:{logOn} Subject:{subject} Mesaage:This message has been received.({data})",
			DateTime.Now.ToString("o"),
			subject,
			data);

		return service.ExecuteAsync(
			command: new FormQueueCommand(
				Id: data.Id.ToGuid()),
			cancellationToken: cancellationToken);
	}
}
