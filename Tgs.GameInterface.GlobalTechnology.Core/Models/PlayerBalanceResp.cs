namespace Tgs.GameInterface.GlobalTechnology.Core.Models;

public record PlayerBalanceResp(
	bool IsPlayerExist,
	string PlayerId,
	string Currency,
	decimal Balance,
	decimal BonusBalance,
	int BrandId);
