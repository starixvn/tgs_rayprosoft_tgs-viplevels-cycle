using Tgs.VipLevels.Cycle.Core.JobApplicationServices;

namespace Tgs.VipLevels.Cycle.Core;

public interface IHangfireUtensil
{
    public Task JobCheckAsync();

    public Task JobExecuteAsync(JobExecuteCommand source);

    public Task JobInitAsync();
}
