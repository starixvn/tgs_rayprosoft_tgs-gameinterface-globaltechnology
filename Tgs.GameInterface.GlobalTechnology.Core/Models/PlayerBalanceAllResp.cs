namespace Tgs.GameInterface.GlobalTechnology.Core.Models;

public record PlayerBalanceAllResp(
	string PlayerId,
	string Currency,
	decimal Balance,
	decimal BonusBalance,
	int BrandId);
