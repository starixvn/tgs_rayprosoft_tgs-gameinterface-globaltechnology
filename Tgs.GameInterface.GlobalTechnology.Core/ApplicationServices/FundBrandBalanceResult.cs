namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public record FundBrandBalanceResult(
	int BrandId,
	string Currency,
	decimal Balance,
	string LastTransId,
	DateTimeOffset UpdateAt);
