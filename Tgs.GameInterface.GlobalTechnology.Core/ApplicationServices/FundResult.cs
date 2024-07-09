namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public record FundResult(
	string OrderId,
	string ExternalOrderId,
	decimal Balance,
	string Resp);
