using Tgs.ServiceUtility;
using Valhalla.MessageQueue;
using Tgs.VipLevels.Cycle.Core.Protos;
using Tgs.VipLevels.Cycle.Core.ApplicationServices;

namespace Tgs.VipLevels.Cycle.Messages;

internal class FormCreateMessageHandler(
	ILogger<FormCreateMessageHandler> logger,
	ICommandService<FormCreateCommand> service) : IMessageHandler<VipLevelsCycleBrandMessage>
{
	public ValueTask HandleAsync(string subject, VipLevelsCycleBrandMessage data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("LogOn:{logOn} Subject:{subject} Mesaage:This message has been received.({data})",
			DateTime.Now.ToString("o"),
			subject,
			data);

		return service.ExecuteAsync(
			command: new FormCreateCommand(
				ResellerId: data.ResellerId.ToGuid(),
				ResellerCode: data.ResellerCode,
				LicenseeId: data.LicenseeId.ToGuid(),
				LicenseeCode: data.LicenseeCode,
				BrandId: data.BrandId.ToGuid(),
				BrandCode: data.BrandCode,
				TimeSpanMinutes: data.TimeSpanMinutes,
				CurrentTime: data.CurrentTime.ToDateTimeOffset()),
			cancellationToken: cancellationToken);
	}
}
