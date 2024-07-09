namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public record PlayerBalanceResult(
	bool IsPlayerExist,
	string PlayerCode,
	string Currency,
	decimal Balance,
	decimal BonusBalance,
	int BrandId);
