using Tgs.VipLevels.Cycle.Core.Models;

namespace Tgs.VipLevels.Cycle.Core;
public interface IBrandRepository
{
	IAsyncEnumerable<BrandInfo> QueryAsync(CancellationToken cancellationToken = default);
	ValueTask<BrandInfo> GetAsync(string code, CancellationToken cancellationToken = default);
}
