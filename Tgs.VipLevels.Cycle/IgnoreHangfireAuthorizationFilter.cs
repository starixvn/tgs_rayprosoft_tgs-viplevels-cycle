using Hangfire.Dashboard;

namespace Tgs.VipLevels.Cycle;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}
