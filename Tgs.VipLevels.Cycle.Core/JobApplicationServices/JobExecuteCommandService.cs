using Google.Protobuf.WellKnownTypes;

using Microsoft.Extensions.Logging;

using Tgs.ServiceUtility;
using Tgs.VipLevels.Cycle.Core.Protos;
using Valhalla.MessageQueue;

namespace Tgs.VipLevels.Cycle.Core.JobApplicationServices;

internal class JobExecuteCommandService(
	ILogger<JobExecuteCommandService> logger,
	TimeProvider timeProvider,
	IMessageSender messageSender) : ICommandService<JobExecuteCommand>
{
	public async ValueTask ExecuteAsync(JobExecuteCommand command, CancellationToken cancellationToken = default)
	{
		var message = new VipLevelsCycleBrandMessage
		{
			ResellerId = command.ResellerId,
			ResellerCode = command.ResellerCode,
			LicenseeId = command.LicenseeId,
			LicenseeCode = command.LicenseeCode,
			BrandId = command.BrandId,
			BrandCode = command.BrandCode,
			TimeSpanMinutes = command.TimeSpanMinutes,
			CurrentTime = timeProvider.GetUtcNow().ToTimestamp()
		};
		await messageSender.PublishAsync(
			subject: VipLevelsCycleNatsConst.Create,
			data: message,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} Subject:{subject} Message:This message has been published.({message})",
			DateTime.Now.ToString("u"),
			VipLevelsCycleNatsConst.Create,
			message);
	}
}
