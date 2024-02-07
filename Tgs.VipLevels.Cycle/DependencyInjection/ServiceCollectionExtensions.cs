using Microsoft.EntityFrameworkCore;
using Tgs.PlayerService.Client;
using Tgs.VipLevels.Cycle;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddVipLevelsCycle(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptions,
		Action<RedisOptions, IServiceProvider> redisOptions,
		Action<PlayerServiceClientOptions, IServiceProvider> playerServiceClientOptions)
		=> services
		.AddOptions<RedisOptions>()
		.Configure(redisOptions)
		.Services
		.AddSingleton<RedisConnectionManager>()
		.AddVipLevelServiceClient((httpClientBuilder) =>
		httpClientBuilder.ConfigureHttpClient((sp, http) =>
		{
			var configuration = sp.GetRequiredService<IConfiguration>();
			http.BaseAddress = configuration.GetValue<Uri>("VipLevelConnectionStrings:VipLevelServiceClientBaseUrl")!;
		}))
		.AddPlayerServiceClient(playerServiceClientOptions)
		.AddVipLevelsCycleCore(
			redisDatabaseResolver: (sp) => sp.GetRequiredService<RedisConnectionManager>().Database)
		.AddVipLevelsCycleRepositories(
			dbContextOptions: dbContextOptions);
}
