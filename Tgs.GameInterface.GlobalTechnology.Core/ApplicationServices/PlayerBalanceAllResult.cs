namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public record PlayerBalanceAllResult(
	string PlayerCode,
	string Currency,
	decimal Balance,
	decimal BonusBalance,
	int BrandId);
