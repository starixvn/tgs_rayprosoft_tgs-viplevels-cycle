using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Tgs.ServiceUtility;
using Tgs.VipLevels.Cycle.Core;
using Tgs.VipLevels.Cycle.Core.Protos;
using Tgs.VipLevels.Cycle.ViewModels;
using Tgs.VipLevelService.Common.ViewModels;
using Valhalla.MessageQueue;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Tgs.VipLevels.Cycle.Controllers;

[Route("form")]
[ApiController]
public class PlayerVipLevelCycleFormController(
	ILogger<PlayerVipLevelCycleFormController> logger) : ControllerBase
{
	/// <summary>
	/// Publishes the asynchronous.
	/// </summary>
	/// <param name="repository">The repository.</param>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///		當前時間 2024-01-01T00:00:00+08:00觸發季福利
	///		週期時間 2023-10-01~2024-01-01
	///			{
	///				"brandCode": "GPI",
	///				"currentTime": "2024-01-01T00:01:00+08:00"
	///			}
	/// </remarks>
	[HttpPost("publish", Name = "PlayerVipLevelCycle_Form_Create")]
	public async ValueTask PublishAsync(
			[FromServices] IBrandRepository repository,
			[FromServices] IMessageSender messageSender,
			[FromBody] FormCreateViewModel source)
	{
		var brandInfo = await repository.GetAsync(
			code: source.BrandCode,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		var message = new VipLevelsCycleBrandMessage
		{
			ResellerId = brandInfo.ResellerId,
			ResellerCode = brandInfo.ResellerCode,
			LicenseeId = brandInfo.LicenseeId,
			LicenseeCode = brandInfo.LicenseeCode,
			BrandId = brandInfo.BrandId,
			BrandCode = brandInfo.BrandCode,
			TimeSpanMinutes = 480,
			CurrentTime = source.CurrentTime.ToTimestamp()
		};
		await messageSender.PublishAsync(
			subject: VipLevelsCycleNatsConst.Create,
			data: message,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} Subject:{subject} Message:This message has been published.({message})",
			DateTime.Now.ToString("u"),
			VipLevelsCycleNatsConst.Create,
			message);
	}
}
