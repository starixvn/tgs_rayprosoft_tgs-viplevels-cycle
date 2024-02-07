using System.Reflection;
using StackExchange.Redis;
using Tgs.VipLevels.Cycle.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddVipLevelsCycleCore(
		this IServiceCollection services,
		Func<IServiceProvider, IDatabase> redisDatabaseResolver)
		=> services
		.RegisterAllServiceUtilityTypes(
			Assembly.GetExecutingAssembly(),
			ServiceLifetime.Transient)
		.AddSingleton<IHangfireLock>((sp) => ActivatorUtilities.CreateInstance<HangfireLock>(sp, redisDatabaseResolver(sp)))
		.AddScoped<IHangfireUtensil, HangfireUtensil>()
		.AddSingleton<ISystemClock, SystemClock>();
}
