using System.Diagnostics;
using Tgs.GameInterface.GlobalTechnology;

namespace Tgs.GameInterface.GlobalTechnology;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } = new ActivitySource(typeof(DiagnosticsInterceptorOptions).FullName!);
}
