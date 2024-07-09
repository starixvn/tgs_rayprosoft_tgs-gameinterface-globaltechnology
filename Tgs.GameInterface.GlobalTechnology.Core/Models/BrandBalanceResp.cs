namespace Tgs.GameInterface.GlobalTechnology.Core.Models;

public record BrandBalanceResp(
	int BrandId,
	string Currency,
	decimal Balance,
	string LastTransId,
	DateTimeOffset UpdateAt);
