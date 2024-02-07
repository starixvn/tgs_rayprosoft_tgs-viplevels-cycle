using Microsoft.EntityFrameworkCore;
using Tgs.VipLevels.Cycle.Core;
using Tgs.VipLevels.Cycle.Entities;
using Tgs.VipLevels.Cycle.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddVipLevelsCycleRepositories(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptions)
		=> services
		.AddDbContext<TgsContext>(dbContextOptions)
		.AddTransient<IBrandRepository, BrandRepository>();
}
