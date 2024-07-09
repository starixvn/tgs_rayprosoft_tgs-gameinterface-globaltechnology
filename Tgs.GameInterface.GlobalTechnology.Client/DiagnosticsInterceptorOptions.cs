using System.Diagnostics;
using Tgs.GameInterface.GlobalTechnology.Client;

namespace Tgs.GameInterface.GlobalTechnology.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } = new ActivitySource(typeof(DiagnosticsInterceptorOptions).FullName!);
}
